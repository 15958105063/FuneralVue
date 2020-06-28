using Funeral.Core.IRepository;
using Funeral.Core.IServices;
using Funeral.Core.Model.Models;
using Funeral.Core.Services.BASE;

namespace Funeral.Core.Services
{
    public partial class AchRolServices : BaseServices<AchRol>, IAchRolServices
    {
        private readonly IAchRolRepository _dal;
        public AchRolServices(IAchRolRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
    }
}