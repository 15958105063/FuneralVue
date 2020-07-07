using Funeral.Core.IRepository;
using Funeral.Core.IServices;
using Funeral.Core.Model.Models;
using Funeral.Core.Services.BASE;

namespace Funeral.Core.Services
{
    public partial class AchFgpServices : BaseServices<AchFgp>, IAchFgpServices
    {
        private readonly IAchFgpRepository _dal;
        public AchFgpServices(IAchFgpRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
    }
}