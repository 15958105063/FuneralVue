using Funeral.Core.Services.BASE;
using Funeral.Core.Model.Models;
using Funeral.Core.IRepository;
using Funeral.Core.IServices;

namespace Funeral.Core.Services
{	
	/// <summary>
	/// PermissionServices
	/// </summary>	
	public class PermissionServices : BaseServices<Permission>, IPermissionServices
    {
	
        IPermissionRepository _dal;
        public PermissionServices(IPermissionRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
       
    }
}
