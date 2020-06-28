using Funeral.Core.IRepository;
using Funeral.Core.IServices;
using Funeral.Core.Model.Models;
using Funeral.Core.Services.BASE;

namespace Funeral.Core.Services
{
    public partial class AchSicServices : BaseServices<AchSic>, IAchSicServices
    {
        private readonly IAchSicRepository _dal;
        public AchSicServices(IAchSicRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
    }
}