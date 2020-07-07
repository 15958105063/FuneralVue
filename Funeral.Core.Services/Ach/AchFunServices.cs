using Funeral.Core.IRepository;
using Funeral.Core.IServices;
using Funeral.Core.Model.Models;
using Funeral.Core.Services.BASE;

namespace Funeral.Core.Services
{
    public partial class AchFunServices : BaseServices<AchFun>, IAchFunServices
    {
        private readonly IAchFunRepository _dal;
        public AchFunServices(IAchFunRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
    }
}