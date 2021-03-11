using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Com.Qsw.Module.Room.Interface;

namespace Com.Qsw.Module.Room.Impl
{
    public class RoomInfoService : IRoomInfoService
    {
        private readonly IRoomInfoRepository roomInfoRepository;

        public RoomInfoService(IRoomInfoRepository roomInfoRepository)
        {
            this.roomInfoRepository = roomInfoRepository;
        }

        public async Task<IList<RoomInfo>> LoadAll(int pageNum, int pageSize)
        {
            await Task.CompletedTask;
            lock (this)
            {
                return roomInfoRepository.All().Skip((pageNum - 1) * pageSize).Take(pageSize).ToList();
            }
        }

        public async Task<RoomInfo> Get(long roomId)
        {
            await Task.CompletedTask;
            lock (this)
            {
                return roomInfoRepository.Get(roomId).Result;
            }
        }

        public async Task<RoomInfo> CreateRoom(string userId)
        {
            await Task.CompletedTask;
            lock (this)
            {
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
                return roomInfoRepository.Save(roomInfo).Result;
            }
        }

        public async Task<RoomInfo> JoinRoom(long roomId, string userId)
        {
            await Task.CompletedTask;
            lock (this)
            {
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
                return roomInfoRepository.Update(roomInfo).Result;
            }
        }

        public async Task<RoomInfo> LeaveRoom(long roomId, string userId)
        {
            await Task.CompletedTask;
            lock (this)
            {
                RoomInfo roomInfo = roomInfoRepository.Get(roomId).Result;
                if (roomInfo == null)
                {
                    throw new ArgumentNullException(nameof(roomId));
                }

                if (!roomInfo.RoomUserInfoByUserIdDictionary.ContainsKey(userId))
                {
                    return roomInfo;
                }

                roomInfo.RoomUserInfoByUserIdDictionary.Remove(userId);

                if (roomInfo.OrganizerUserId == userId)
                {
                    if (roomInfo.RoomUserInfoByUserIdDictionary.Count > 0)
                    {
                        roomInfo.OrganizerUserId = roomInfo.RoomUserInfoByUserIdDictionary.Values
                            .OrderBy(m => m.JoinDateTime).First().UserId;
                    }
                    else
                    {
                        roomInfo.OrganizerUserId = null;
                    }
                }

                return roomInfoRepository.Update(roomInfo).Result;
            }
        }

        public async Task<RoomInfo> DeleteRoom(long roomId)
        {
            await Task.CompletedTask;
            lock (this)
            {
                RoomInfo roomInfo = roomInfoRepository.Get(roomId).Result;
                if (roomInfo == null)
                {
                    return null;
                }

                roomInfoRepository.Delete(roomId).Wait();
                return roomInfo;
            }
        }
    }
}