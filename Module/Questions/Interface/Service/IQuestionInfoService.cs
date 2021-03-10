using System.Threading.Tasks;

namespace Com.Qsw.Module.Question.Interface
{
    public interface IQuestionInfoService
    {
        /// <summary>
        /// random select a question based on question category and difficult.
        /// </summary>
        /// <param name="questionCategory"> Question Category </param>
        /// <param name="minDifficult"> Min difficult, this service will select the question which difficult between difficult and difficult + 10 </param>
        /// <returns>Selected question info.</returns>
        Task<QuestionInfo> RandomSelect(QuestionCategory questionCategory, int minDifficult);
    }
}