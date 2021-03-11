using System.Threading.Tasks;
using Com.Qsw.Module.Game.Interface;

namespace Com.Qsw.Module.Game.Action
{
    public interface IGameActionService
    {
        Task<GameInfo> RetrieveQuestion(long gameId, string userId);
        Task SubmitAnswer(long gameId, string userId, string answer);
        Task LeftGame(long gameId, string userId);
    }
}