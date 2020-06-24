using Funeral.Core.IServices;
using Funeral.Core.IRepository;
using Funeral.Core.Services.BASE;
using Funeral.Core.Model.Models;
namespace Funeral.Core.Services
{
    /// <summary>
    /// AchOrgServices
    /// </summary>	
    public class AchOrgServices : BaseServices<AchOrg>,IAchOrgServices
    {

        IAchOrgRepository _dal;
        public AchOrgServices(IAchOrgRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
  

    }
}
