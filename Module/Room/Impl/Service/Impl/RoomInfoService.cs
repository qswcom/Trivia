using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Com.Qsw.Framework.MessageQueue.Interface;
using Com.Qsw.Framework.Session.Interface;
using Com.Qsw.Module.Room.Interface;

namespace Com.Qsw.Module.Room.Impl
{
    public class RoomInfoService : IRoomInfoService
    {
        private readonly IRoomInfoRepository roomInfoRepository;
        private readonly IMessageService messageService;

        public RoomInfoService(IRoomInfoRepository roomInfoRepository, IMessageService messageService)
        {
            this.roomInfoRepository = roomInfoRepository;
            this.messageService = messageService;
        }

        [Lock]
        public async Task<IList<RoomInfo>> LoadAll(int pageNum, int pageSize)
        {
            await Task.CompletedTask;
            if (pageNum <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pageNum));
            }

            if (pageSize <= 0 || pageSize > RoomConstants.MaxRoomLoadAllNum)
            {
                throw new ArgumentOutOfRangeException(nameof(pageSize));
            }

            return roomInfoRepository.All().Skip((pageNum - 1) * pageSize).Take(pageSize).ToList();
        }

        [Lock]
        public async Task<RoomInfo> Get(long roomId)
        {
            if (roomId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(roomId));
            }

            return await roomInfoRepository.Get(roomId);
        }

        [Lock]
        public async Task<RoomInfo> CreateRoom(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentNullException(nameof(userId));
            }

            var roomUserInfo = new RoomUserInfo
            {
                UserId = userId,
                RoomUserRole = RoomUserRole.Organizer,
                JoinDateTime = DateTime.UtcNow
            };
            var roomInfo = new RoomInfo
            {
                OrganizerUserId = userId,
            };
            roomInfo.RoomUserInfoByUserIdDictionary[userId] = roomUserInfo;
            RoomInfo result = await roomInfoRepository.Save(roomInfo);
            await SendEntityChangedMessage(new RoomChangedMessage(roomInfo, OperationType.Save));
            return result;
        }

        [Lock]
        public async Task<RoomInfo> JoinRoom(long roomId, string userId)
        {
            if (roomId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(roomId));
            }

            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentNullException(nameof(userId));
            }

            RoomInfo roomInfo = roomInfoRepository.Get(roomId).Result;
            if (roomInfo == null)
            {
                throw new ArgumentNullException(nameof(roomId));
            }

            if (roomInfo.RoomUserInfoByUserIdDictionary.ContainsKey(userId))
            {
                return roomInfo;
            }

            var roomUserInfo = new RoomUserInfo
            {
                UserId = userId,
                RoomUserRole = RoomUserRole.NormalUser,
                JoinDateTime = DateTime.UtcNow
            };
            roomInfo.RoomUserInfoByUserIdDictionary[userId] = roomUserInfo;
            RoomInfo result = await roomInfoRepository.Update(roomInfo);
            await SendEntityChangedMessage(new RoomChangedMessage(roomInfo, OperationType.Update));
            return result;
        }

        [Lock]
        public async Task<RoomInfo> LeaveRoom(long roomId, string userId)
        {
            if (roomId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(roomId));
            }

            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentNullException(nameof(userId));
            }

            RoomInfo roomInfo = roomInfoRepository.Get(roomId).Result;
            if (roomInfo == null)
            {
                throw new ArgumentNullException(nameof(roomId));
            }

            if (!roomInfo.RoomUserInfoByUserIdDictionary.ContainsKey(userId))
            {
                return roomInfo;
            }

            if (roomInfo.RoomUserInfoByUserIdDictionary.Count == 1)
            {
                return await DeleteRoom(roomId);
            }

            roomInfo.RoomUserInfoByUserIdDictionary.Remove(userId);
            if (roomInfo.OrganizerUserId == userId)
            {
                roomInfo.OrganizerUserId = roomInfo.RoomUserInfoByUserIdDictionary.Values
                    .OrderBy(m => m.JoinDateTime).First().UserId;
            }

            RoomInfo result = await roomInfoRepository.Update(roomInfo);
            await SendEntityChangedMessage(new RoomChangedMessage(roomInfo, OperationType.Update));
            return result;
        }

        [Lock]
        public async Task<RoomInfo> DeleteRoom(long roomId)
        {
            if (roomId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(roomId));
            }

            RoomInfo roomInfo = roomInfoRepository.Get(roomId).Result;
            if (roomInfo == null)
            {
                return null;
            }

            await roomInfoRepository.Delete(roomId);
            await SendEntityChangedMessage(new RoomChangedMessage(roomInfo, OperationType.Delete));
            return roomInfo;
        }

        #region Message

        private async Task SendEntityChangedMessage(RoomChangedMessage roomChangedMessage)
        {
            await messageService.Publish(RoomChangedMessageTopic.Topic, null, roomChangedMessage);
        }

        #endregion
    }
}