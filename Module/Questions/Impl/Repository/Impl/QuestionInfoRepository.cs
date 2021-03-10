using Com.Qsw.Framework.Session.Impl;
using Com.Qsw.Framework.Session.Interface;
using Com.Qsw.Module.Question.Interface;

namespace Com.Qsw.Module.Question.Impl
{
    public class QuestionInfoRepository : EntityRepository<QuestionInfo>, IQuestionInfoRepository
    {
        public QuestionInfoRepository(ISessionService sessionService) : base(sessionService)
        {
        }
    }
}