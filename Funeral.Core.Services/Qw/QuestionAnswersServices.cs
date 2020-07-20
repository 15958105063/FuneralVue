using Funeral.Core.IRepository;
using Funeral.Core.IServices;
using Funeral.Core.Model.Models;
using Funeral.Core.Services.BASE;

namespace Funeral.Core.Services
{
    public partial class QuestionAnswersServices : BaseServices<QuestionAnswers>, IQuestionAnswersServices
    {
        private readonly IQuestionAnswersRepository _dal;
        public QuestionAnswersServices(IQuestionAnswersRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
    }
}