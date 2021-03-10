using System.Threading.Tasks;
using Com.Qsw.Module.Question.Interface;

namespace Com.Qsw.Module.Question.Impl
{
    public class QuestionInfoServicePermissionDecorator : IQuestionInfoService
    {
        private readonly IQuestionInfoService decoratedService;

        public QuestionInfoServicePermissionDecorator(IQuestionInfoService decoratedService)
        {
            this.decoratedService = decoratedService;
        }

        public Task<QuestionInfo> RandomSelect(QuestionCategory questionCategory, int minDifficult)
        {
            //TODO: Add permission check later.

            return decoratedService.RandomSelect(questionCategory, minDifficult);
        }
    }
}