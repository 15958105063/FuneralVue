using Funeral.Core.IRepository;
using Funeral.Core.IServices;
using Funeral.Core.Model.Models;
using Funeral.Core.Services.BASE;

namespace Funeral.Core.Services
{
    public partial class AchRacServices : BaseServices<AchRac>, IAchRacServices
    {
        private readonly IAchRacRepository _dal;
        public AchRacServices(IAchRacRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
    }
}