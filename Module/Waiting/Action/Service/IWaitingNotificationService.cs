using System.Collections.Generic;
using System.Threading.Tasks;
using Com.Qsw.Module.Room.Interface;

namespace Com.Qsw.Module.Waiting.Action
{
    public interface IWaitingNotificationService
    {
        Task NotifyRoomChanged(RoomChangedMessage roomChangedMessage, IList<string> userIds);
    }
}