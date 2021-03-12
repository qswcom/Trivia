using System.Threading.Tasks;

namespace Com.Qsw.TriviaClient.ConsoleClient.Main
{
    public interface IGameService
    {
        Task<GameInfo> RetrieveQuestion(long gameId);
        Task SubmitAnswer(long gameId, string answer);
        Task LeftGame(long gameId);
    }
}