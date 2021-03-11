using System.Threading.Tasks;
using Com.Qsw.Module.Game.Interface;
using Com.Qsw.Module.Question.Interface;

namespace Com.Qsw.Module.Game.Impl
{
    public interface IGameQuestionService
    {
        Task<QuestionInfo> GetNextQuestionInfo(GameInfo gameInfo);

        Task<bool> IsRightAnswer(GameInfo gameInfo, string answer);
    }
}