using Funeral.Core.IRepository;
using Funeral.Core.IServices;
using Funeral.Core.Model.Models;
using Funeral.Core.Services.BASE;

namespace Funeral.Core.Services
{
    public partial class AchUsrServices : BaseServices<AchUsr>, IAchUsrServices
    {
        private readonly IAchUsrRepository _dal;
        public AchUsrServices(IAchUsrRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
    }
}