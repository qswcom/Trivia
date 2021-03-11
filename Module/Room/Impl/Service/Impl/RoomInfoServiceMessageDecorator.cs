using System.Collections.Generic;
using System.Threading.Tasks;
using Com.Qsw.Framework.MessageQueue.Interface;
using Com.Qsw.Framework.Session.Interface;
using Com.Qsw.Module.Room.Interface;

namespace Com.Qsw.Module.Room.Impl
{
    public class RoomInfoServiceMessageDecorator : IRoomInfoService
    {
        private readonly IRoomInfoService decoratedService;
        private readonly IMessageService messageService;

        public RoomInfoServiceMessageDecorator(IRoomInfoService decoratedService,
            IMessageService messageService)
        {
            this.decoratedService = decoratedService;
            this.messageService = messageService;
        }

        public Task<IList<RoomInfo>> LoadAll(int pageNum, int pageSize)
        {
            return decoratedService.LoadAll(pageNum, pageSize);
        }

        public Task<RoomInfo> Get(long roomId)
        {
            return decoratedService.Get(roomId);
        }

        public async Task<RoomInfo> CreateRoom(string userId)
        {
            RoomInfo roomInfo = await decoratedService.CreateRoom(userId);
            await SendEntityChangedMessage(new RoomChangedMessage(roomInfo, OperationType.Save));
            return roomInfo;
        }

        public async Task<RoomInfo> JoinRoom(long roomId, string userId)
        {
            RoomInfo roomInfo = await decoratedService.JoinRoom(roomId, userId);
            await SendEntityChangedMessage(new RoomChangedMessage(roomInfo, OperationType.Update));
            return roomInfo;
        }

        public async Task<RoomInfo> LeaveRoom(long roomId, string userId)
        {
            RoomInfo roomInfo = await decoratedService.LeaveRoom(roomId, userId);
            await SendEntityChangedMessage(new RoomChangedMessage(roomInfo, OperationType.Update));
            return roomInfo;
        }

        public async Task<RoomInfo> DeleteRoom(long roomId)
        {
            RoomInfo roomInfo = await decoratedService.DeleteRoom(roomId);
            if (roomInfo != null)
            {
                await SendEntityChangedMessage(new RoomChangedMessage(roomInfo, OperationType.Delete));
            }

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