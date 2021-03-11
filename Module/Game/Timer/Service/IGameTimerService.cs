using System.Threading.Tasks;
using Com.Qsw.Module.Game.Interface;

namespace Com.Qsw.Module.Game.Timer.Service
{
    public interface IGameTimerService
    {
        Task AddTimer(GameInfo gameInfo, string userId);
    }
}