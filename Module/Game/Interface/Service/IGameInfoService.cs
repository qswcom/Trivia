using System.Collections.Generic;
using System.Threading.Tasks;

namespace Com.Qsw.Module.Game.Interface
{
    public interface IGameInfoService
    {
        Task<GameInfo> Create(IList<string> userIds);
        Task<GameInfo> RetrieveQuestion(long gameId, string userId);
        Task SubmitAnswer(long gameId, string userId, string answer);
        Task LeftGame(long gameId, string userId);
        Task OnTimeExpired(long gameId, string userId);
    }
}