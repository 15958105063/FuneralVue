using Funeral.Core.IRepository;
using Funeral.Core.IServices;
using Funeral.Core.Model.Models;
using Funeral.Core.Services.BASE;

namespace Funeral.Core.Services
{
    public partial class AchSitServices : BaseServices<AchSit>, IAchSitServices
    {
        private readonly IAchSitRepository _dal;
        public AchSitServices(IAchSitRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
    }
}