using Funeral.Core.IRepository;
using Funeral.Core.IServices;
using Funeral.Core.Model.Models;
using Funeral.Core.Services.BASE;

namespace Funeral.Core.Services
{
    public partial class AchVdrServices : BaseServices<AchVdr>, IAchVdrServices
    {
        private readonly IAchVdrRepository _dal;
        public AchVdrServices(IAchVdrRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
    }
}