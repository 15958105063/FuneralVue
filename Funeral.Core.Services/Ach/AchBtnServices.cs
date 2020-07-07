using Funeral.Core.IRepository;
using Funeral.Core.IServices;
using Funeral.Core.Model.Models;
using Funeral.Core.Services.BASE;

namespace Funeral.Core.Services
{
    public partial class AchBtnServices : BaseServices<AchBtn>, IAchBtnServices
    {
        private readonly IAchBtnRepository _dal;
        public AchBtnServices(IAchBtnRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
    }
}