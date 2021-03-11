using System.Threading.Tasks;

namespace Com.Qsw.TriviaClient.ConsoleClient.Main
{
    public interface IUserStateInfoService
    {
        Task<UserStateInfo> GetOrCreate();
    }
}