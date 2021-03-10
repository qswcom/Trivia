using Com.Qsw.Framework.Session.Impl;
using Com.Qsw.Framework.Session.Interface;

namespace Com.Qsw.Module.Question.Impl
{
    public class QuestionInfoStatisticRepository : EntityRepository<QuestionInfoStatistic>,
        IQuestionInfoStatisticRepository
    {
        public QuestionInfoStatisticRepository(ISessionService sessionService) : base(sessionService)
        {
        }
    }
}