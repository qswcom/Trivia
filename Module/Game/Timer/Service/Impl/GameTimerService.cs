using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Com.Qsw.Framework.Context.Web;
using Com.Qsw.Framework.MessageQueue.Interface;
using Com.Qsw.Module.Game.Interface;
using Com.Qsw.Module.Game.Timer.Service;
using Microsoft.Extensions.Logging;

namespace Com.Qsw.Module.Game.Timer
{
    public class GameTimerService : IGameTimerService, IMonitor
    {
        private readonly ILogger logger;
        private readonly IMessageService messageService;
        private readonly Queue<DateTime> dateTimeQueue;
        private readonly IDictionary<DateTime, LinkedList<GameUserTimeInfo>> gameUserTimeInfosByDateTimeDic;
        private DateTime startDateTime;
        private DateTime endDateTime;

        public GameTimerService(ILoggerFactory loggerFactory, IMessageService messageService)
        {
            logger = loggerFactory.CreateLogger<GameTimerService>();
            this.messageService = messageService;

            dateTimeQueue = new Queue<DateTime>();
            gameUserTimeInfosByDateTimeDic = new Dictionary<DateTime, LinkedList<GameUserTimeInfo>>();
            DateTime utcNow = DateTime.UtcNow;
            startDateTime = TrimMilli(utcNow);
            endDateTime = startDateTime.Add(TimeSpan.FromSeconds(GameTimerConstants.MaxInternalInSeconds));
            for (DateTime dateTime = startDateTime;
                dateTime <= endDateTime;
                dateTime = dateTime.AddSeconds(1))
            {
                dateTimeQueue.Enqueue(dateTime);
                gameUserTimeInfosByDateTimeDic[dateTime] = new LinkedList<GameUserTimeInfo>();
            }
        }

        public void Start()
        {
            var thread = new Thread(Monitor);
            thread.IsBackground = true;
            thread.Start();
        }

        public Task AddTimer(GameInfo gameInfo, string userId)
        {
            DateTime expireDateTime = gameInfo.GameUserInfoByUserIdDictionary[userId].GameUserQuestionInfoList.Last()
                .ExpireDateTime;
            if (expireDateTime < startDateTime || expireDateTime > endDateTime)
            {
                throw new ArgumentOutOfRangeException(nameof(gameInfo), "Time exceed range.");
            }

            lock (this)
            {
                DateTime dateTime = TrimMilli(expireDateTime);
                gameUserTimeInfosByDateTimeDic[dateTime].AddLast(
                    new GameUserTimeInfo
                    {
                        GameId = gameInfo.Id,
                        UserId = userId,
                        QuestionIndex = gameInfo.GameQuestionInfo.QuestionInfoList.Count - 1,
                        TriggerDateTime = expireDateTime
                    });
            }

            return Task.CompletedTask;
        }

        #region Monitor

        private void Monitor()
        {
            while (true)
            {
                DateTime utcNow = DateTime.UtcNow;
                IList<GameUserTimeInfo> gameUserTimeInfos = new List<GameUserTimeInfo>();
                lock (this)
                {
                    DateTime utcNowWithoutMilli = TrimMilli(utcNow);
                    while (startDateTime < utcNowWithoutMilli)
                    {
                        if (dateTimeQueue.Count > 1)
                        {
                            DateTime dateTime = dateTimeQueue.Dequeue();
                            LinkedList<GameUserTimeInfo> userTimeInfos = gameUserTimeInfosByDateTimeDic[dateTime];
                            foreach (GameUserTimeInfo gameUserTimeInfo in userTimeInfos)
                            {
                                gameUserTimeInfos.Add(gameUserTimeInfo);
                            }

                            gameUserTimeInfosByDateTimeDic.Remove(dateTime);
                            startDateTime = dateTimeQueue.Peek();
                        }
                        else
                        {
                            break;
                        }
                    }

                    LinkedList<GameUserTimeInfo> gameUseTimeInfoLinkedList =
                        gameUserTimeInfosByDateTimeDic[startDateTime];
                    LinkedListNode<GameUserTimeInfo> current = gameUseTimeInfoLinkedList.First;
                    while (current != null)
                    {
                        if (current.Value.TriggerDateTime <= utcNow)
                        {
                            gameUserTimeInfos.Add(current.Value);
                            LinkedListNode<GameUserTimeInfo> temp = current.Next;
                            gameUseTimeInfoLinkedList.Remove(current);
                            current = temp;
                        }
                        else
                        {
                            current = current.Next;
                        }
                    }

                    DateTime nextEndDateTime =
                        startDateTime.Add(TimeSpan.FromSeconds(GameTimerConstants.MaxInternalInSeconds));
                    while (true)
                    {
                        DateTime nextDateTime = endDateTime.AddSeconds(1);
                        if (nextDateTime > nextEndDateTime)
                        {
                            break;
                        }

                        dateTimeQueue.Enqueue(nextDateTime);
                        gameUserTimeInfosByDateTimeDic[nextDateTime] = new LinkedList<GameUserTimeInfo>();
                        endDateTime = nextDateTime;
                    }
                }

                try
                {
                    SendMessages(gameUserTimeInfos);
                }
                catch (Exception e)
                {
                    logger.LogError(e, "Error on send time message.");
                }
                finally
                {
                    Thread.Sleep(TimeSpan.FromMilliseconds(40));
                }
            }

        }

        private void SendMessages(IList<GameUserTimeInfo> gameUserTimeInfos)
        {
            if (gameUserTimeInfos.Count == 0)
            {
                return;
            }

            foreach (GameUserTimeInfo gameUserTimeInfo in gameUserTimeInfos)
            {
                Task.Run(async () =>
                {
                    try
                    {
                        await messageService.Publish(GameTimeExpiredTopic.Topic, null,
                            new GameTimeExpiredMessage
                            {
                                GameId = gameUserTimeInfo.GameId,
                                UserId = gameUserTimeInfo.UserId,
                                QuestionIndex = gameUserTimeInfo.QuestionIndex
                            });
                    }
                    catch (Exception e)
                    {
                        logger.LogError(e, "Error on send notification to user.");
                    }
                });
            }
        }

        #endregion

        #region Helper

        private DateTime TrimMilli(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute,
                dateTime.Second, dateTime.Kind);
        }

        #endregion


        #region Define

        private class GameUserTimeInfo
        {
            public long GameId { get; set; }
            public string UserId { get; set; }
            public int QuestionIndex { get; set; }
            public DateTime TriggerDateTime { get; set; }
        }

        #endregion
    }
}