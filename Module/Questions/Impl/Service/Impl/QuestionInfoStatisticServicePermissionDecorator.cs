using System.Threading.Tasks;
using Com.Qsw.Framework.Session.Interface;
using Com.Qsw.Module.Question.Interface;

namespace Com.Qsw.Module.Question.Impl
{
    public class QuestionInfoStatisticServicePermissionDecorator : IQuestionInfoStatisticService
    {
        private readonly IQuestionInfoStatisticService decoratedService;

        public QuestionInfoStatisticServicePermissionDecorator(IQuestionInfoStatisticService decoratedService)
        {
            this.decoratedService = decoratedService;
        }

        [Transaction(true)]
        public async Task<int> GetQuestionCount(QuestionCategory questionCategory, int minDifficult)
        {
            //TODO: Add permission check later.
            return await decoratedService.GetQuestionCount(questionCategory, minDifficult);
        }
    }
}