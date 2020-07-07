using Funeral.Core.IRepository;
using Funeral.Core.IServices;
using Funeral.Core.Model.Models;
using Funeral.Core.Services.BASE;

namespace Funeral.Core.Services
{
    public partial class AchFupServices : BaseServices<AchFup>, IAchFupServices
    {
        private readonly IAchFupRepository _dal;
        public AchFupServices(IAchFupRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
    }
}