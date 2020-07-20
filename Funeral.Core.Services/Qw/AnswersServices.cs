using Funeral.Core.IRepository;
using Funeral.Core.IServices;
using Funeral.Core.Model.Models;
using Funeral.Core.Services.BASE;

namespace Funeral.Core.Services
{
    public partial class AnswersServices : BaseServices<Answers>, IAnswersServices
    {
        private readonly IAnswersRepository _dal;
        public AnswersServices(IAnswersRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
    }
}