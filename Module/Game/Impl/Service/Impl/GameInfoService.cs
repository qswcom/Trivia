using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Com.Qsw.Framework.MessageQueue.Interface;
using Com.Qsw.Framework.Session.Interface;
using Com.Qsw.Module.Game.Interface;
using Com.Qsw.Module.Game.Timer.Service;
using Com.Qsw.Module.Question.Interface;

namespace Com.Qsw.Module.Game.Impl
{
    public class GameInfoService : IGameInfoService
    {
        private readonly IGameInfoRepository gameInfoRepository;
        private readonly IMessageService messageService;
        private readonly IGameQuestionService gameQuestionService;
        private readonly IGameTimerService gameTimerService;
        private readonly IGameStateService gameStateService;

        public GameInfoService(IGameInfoRepository gameInfoRepository, IMessageService messageService,
            IGameQuestionService gameQuestionService, IGameTimerService gameTimerService,
            IGameStateService gameStateService)
        {
            this.gameInfoRepository = gameInfoRepository;
            this.messageService = messageService;
            this.gameQuestionService = gameQuestionService;
            this.gameTimerService = gameTimerService;
            this.gameStateService = gameStateService;
        }

        [Lock]
        public async Task<GameInfo> Create(IList<string> userIds)
        {
            if (userIds.Count <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(userIds));
            }

            var gameInfo = new GameInfo();

            QuestionInfo questionInfo = await gameQuestionService.GetNextQuestionInfo(gameInfo);
            gameInfo.GameQuestionInfo.QuestionInfoList.Add(questionInfo);
            foreach (string userId in userIds)
            {
                var gameUserInfo = new GameUserInfo
                {
                    UserId = userId,
                    GameUserState = GameUserState.Continue
                };
                var gameUserQuestionInfo = new GameUserQuestionInfo
                {
                    GameUserQuestionState = GameUserQuestionState.NotStart,
                };
                gameUserInfo.GameUserQuestionInfoList.Add(gameUserQuestionInfo);
                gameInfo.GameUserInfoByUserIdDictionary[userId] = gameUserInfo;
            }

            gameInfo.GameState = GameState.Continue;
            GameInfo savedGameInfo = await gameInfoRepository.Save(gameInfo);
            await SendEntityChangedMessage(new GameInfoChangedMessage(savedGameInfo, OperationType.Save));
            return savedGameInfo;
        }

        [Lock]
        public async Task<GameInfo> RetrieveQuestion(long gameId, string userId)
        {
            GameInfo gameInfo = await gameInfoRepository.Get(gameId);
            if (gameInfo == null)
            {
                throw new ArgumentOutOfRangeException(nameof(gameId));
            }

            if (!gameInfo.GameUserInfoByUserIdDictionary.ContainsKey(userId))
            {
                throw new ArgumentOutOfRangeException(nameof(userId), "This user not belongs to this game.");
            }

            if (gameInfo.GameState == GameState.Finished)
            {
                return gameInfo;
            }

            GameUserInfo gameUserInfo = gameInfo.GameUserInfoByUserIdDictionary[userId];
            if (gameUserInfo.GameUserState == GameUserState.Failed)
            {
                return gameInfo;
            }

            GameUserQuestionInfo gameUserQuestionInfo = gameUserInfo.GameUserQuestionInfoList.Last();
            if (gameUserQuestionInfo.GameUserQuestionState == GameUserQuestionState.NotStart)
            {
                gameUserQuestionInfo.ExpireDateTime = DateTime.UtcNow.Add(GameConstants.QuestionAnswerTimeSpan);
                gameUserQuestionInfo.GameUserQuestionState = GameUserQuestionState.Process;
                gameInfo = await gameInfoRepository.Update(gameInfo);
                await gameTimerService.AddTimer(gameInfo, userId);
                return gameInfo;
            }

            return gameInfo;
        }

        [Lock]
        public async Task SubmitAnswer(long gameId, string userId, string answer)
        {
            GameInfo gameInfo = await gameInfoRepository.Get(gameId);
            if (gameInfo == null)
            {
                throw new ArgumentOutOfRangeException(nameof(gameId));
            }

            if (!gameInfo.GameUserInfoByUserIdDictionary.ContainsKey(userId))
            {
                throw new ArgumentOutOfRangeException(nameof(userId), "This user not belongs to this game.");
            }

            if (gameInfo.GameState == GameState.Finished)
            {
                throw new ArgumentOutOfRangeException(nameof(answer), "Game already finished.");
            }

            GameUserInfo gameUserInfo = gameInfo.GameUserInfoByUserIdDictionary[userId];
            if (gameUserInfo.GameUserState == GameUserState.Failed)
            {
                throw new ArgumentOutOfRangeException(nameof(answer), "User already failed.");
            }

            GameUserQuestionInfo gameUserQuestionInfo = gameUserInfo.GameUserQuestionInfoList.Last();
            if (gameUserQuestionInfo.GameUserQuestionState == GameUserQuestionState.NotStart ||
                gameUserQuestionInfo.GameUserQuestionState == GameUserQuestionState.Process)
            {
                gameUserQuestionInfo.AnsweredDateTime = DateTime.UtcNow;
                bool isRightAnswer = await gameQuestionService.IsRightAnswer(gameInfo, answer);
                if (isRightAnswer)
                {
                    gameUserQuestionInfo.GameUserQuestionState = GameUserQuestionState.Success;
                }
                else
                {
                    gameUserQuestionInfo.GameUserQuestionState = GameUserQuestionState.Failed;
                }

                gameInfo = gameStateService.AnalyzeByGameUserQuestionStateChanged(gameInfo, gameUserInfo,
                    gameUserQuestionInfo, out GameResult gameResult, out bool isRoundFinish);
                gameInfo = await HandleStateChangedResult(gameInfo, gameResult, isRoundFinish);
                gameInfo = await gameInfoRepository.Update(gameInfo);
                await SendEntityChangedMessage(new GameInfoChangedMessage(gameInfo, OperationType.Update));
                return;
            }

            throw new ArgumentOutOfRangeException(nameof(answer), "User can't answer this question anymore.");
        }

        [Lock]
        public async Task LeftGame(long gameId, string userId)
        {
            GameInfo gameInfo = await gameInfoRepository.Get(gameId);
            if (gameInfo == null)
            {
                return;
            }

            if (!gameInfo.GameUserInfoByUserIdDictionary.ContainsKey(userId))
            {
                return;
            }

            GameUserInfo gameUserInfo = gameInfo.GameUserInfoByUserIdDictionary[userId];
            GameUserQuestionInfo gameUserQuestionInfo = gameUserInfo.GameUserQuestionInfoList.Last();
            if (gameUserQuestionInfo.GameUserQuestionState == GameUserQuestionState.NotStart ||
                gameUserQuestionInfo.GameUserQuestionState == GameUserQuestionState.Process)
            {
                gameUserQuestionInfo.GameUserQuestionState = GameUserQuestionState.Overtime;
                gameInfo = gameStateService.AnalyzeByGameUserQuestionStateChanged(gameInfo, gameUserInfo,
                    gameUserQuestionInfo, out GameResult gameResult, out bool isRoundFinish);
                gameInfo = await HandleStateChangedResult(gameInfo, gameResult, isRoundFinish);
            }
            else
            {
                gameUserInfo.GameUserState = GameUserState.Failed;
                gameInfo = gameStateService.AnalyzeByGameUserStateChanged(gameInfo, gameUserInfo,
                    out GameResult gameResult, out bool isRoundFinish);
                gameInfo = await HandleStateChangedResult(gameInfo, gameResult, isRoundFinish);
            }

            gameInfo = await gameInfoRepository.Update(gameInfo);
            await SendEntityChangedMessage(new GameInfoChangedMessage(gameInfo, OperationType.Update));
        }

        [Lock]
        public async Task OnTimeExpired(long gameId, string userId)
        {
            GameInfo gameInfo = await gameInfoRepository.Get(gameId);
            if (gameInfo == null)
            {
                throw new ArgumentOutOfRangeException(nameof(gameId));
            }

            if (!gameInfo.GameUserInfoByUserIdDictionary.ContainsKey(userId))
            {
                throw new ArgumentOutOfRangeException(nameof(userId), "This user not belongs to this game.");
            }

            if (gameInfo.GameState == GameState.Finished)
            {
                return;
            }

            GameUserInfo gameUserInfo = gameInfo.GameUserInfoByUserIdDictionary[userId];
            if (gameUserInfo.GameUserState == GameUserState.Failed)
            {
                return;
            }

            GameUserQuestionInfo gameUserQuestionInfo = gameUserInfo.GameUserQuestionInfoList.Last();
            if (gameUserQuestionInfo.GameUserQuestionState == GameUserQuestionState.NotStart ||
                gameUserQuestionInfo.GameUserQuestionState == GameUserQuestionState.Process)
            {
                gameUserQuestionInfo.AnsweredDateTime = DateTime.UtcNow;
                gameUserQuestionInfo.GameUserQuestionState = GameUserQuestionState.Overtime;

                gameInfo = gameStateService.AnalyzeByGameUserQuestionStateChanged(gameInfo, gameUserInfo,
                    gameUserQuestionInfo, out GameResult gameResult, out bool isRoundFinish);
                gameInfo = await HandleStateChangedResult(gameInfo, gameResult, isRoundFinish);
                gameInfo = await gameInfoRepository.Update(gameInfo);
                await SendEntityChangedMessage(new GameInfoChangedMessage(gameInfo, OperationType.Update));
            }
        }

        #region State Management

        private async Task<GameInfo> HandleStateChangedResult(GameInfo gameInfo, GameResult gameResult,
            bool isRoundFinish)
        {
            if (gameResult != null)
            {
                gameInfo.GameResult = gameResult;
                return gameInfo;
            }

            if (isRoundFinish)
            {
                QuestionInfo questionInfo = await gameQuestionService.GetNextQuestionInfo(gameInfo);
                gameInfo.GameQuestionInfo.QuestionInfoList.Add(questionInfo);
                foreach (GameUserInfo gameUserInfo in gameInfo.GameUserInfoByUserIdDictionary.Values)
                {
                    var gameUserQuestionInfo = new GameUserQuestionInfo
                    {
                        GameUserQuestionState = GameUserQuestionState.NotStart,
                    };
                    if (gameUserInfo.GameUserState == GameUserState.Failed)
                    {
                        gameUserQuestionInfo.GameUserQuestionState = GameUserQuestionState.UnApply;
                    }

                    gameUserInfo.GameUserQuestionInfoList.Add(gameUserQuestionInfo);
                }
            }

            return gameInfo;
        }

        #endregion


        #region Message

        private async Task SendEntityChangedMessage(GameInfoChangedMessage gameInfoChangedMessage)
        {
            await messageService.Publish(GameInfoChangedMessageTopic.Topic, null, gameInfoChangedMessage);
        }

        #endregion
    }
}