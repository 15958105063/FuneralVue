using Funeral.Core.IServices.BASE;
using Funeral.Core.Model.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Funeral.Core.IServices
{
    /// <summary>
    /// RoleModulePermissionServices
    /// </summary>	
    public interface IRoleModulePermissionServices :IBaseServices<RoleModulePermission>
	{

        Task<List<RoleModulePermission>> GetRoleModule();
        Task<List<TestMuchTableResult>> QueryMuchTable();
        Task<List<RoleModulePermission>> RoleModuleMaps();
        Task<List<RoleModulePermission>> GetRMPMaps();
    }
}
