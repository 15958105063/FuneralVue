using Funeral.Core.IRepository;
using Funeral.Core.IServices;
using Funeral.Core.Model.Models;
using Funeral.Core.Services.BASE;

namespace Funeral.Core.Services
{
    public partial class AchDptServices : BaseServices<AchDpt>, IAchDptServices
    {
        private readonly IAchDptRepository _dal;
        public AchDptServices(IAchDptRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
    }
}