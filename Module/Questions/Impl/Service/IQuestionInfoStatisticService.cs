using System.Threading.Tasks;
using Com.Qsw.Module.Question.Interface;

namespace Com.Qsw.Module.Question.Impl
{
    public interface IQuestionInfoStatisticService
    {
        Task<int> GetQuestionCount(QuestionCategory questionCategory, int minDifficult);
    }
}