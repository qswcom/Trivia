using System.Threading.Tasks;

namespace Com.Qsw.TriviaServer.AppServer.Main.Test
{
    public interface IDatabaseInitService
    {
        Task InitDatabase();
    }
}