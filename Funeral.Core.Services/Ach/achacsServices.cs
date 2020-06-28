using Funeral.Core.IRepository;
using Funeral.Core.IServices;
using Funeral.Core.Model.Models;
using Funeral.Core.Services.BASE;

namespace Funeral.Core.Services
{
    public partial class AchAcsServices : BaseServices<AchAcs>, IAchAcsServices
    {
        private readonly IAchAcsRepository _dal;
        public AchAcsServices(IAchAcsRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
    }
}