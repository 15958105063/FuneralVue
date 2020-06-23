using Funeral.Core.IRepository.Base;
using Funeral.Core.Model.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Funeral.Core.IRepository
{
    /// <summary>
    /// IRoleModulePermissionRepository
    /// </summary>	
    public interface IRoleModulePermissionRepository : IBaseRepository<RoleModulePermission>//类名
    {
        Task<List<TestMuchTableResult>> QueryMuchTable();
        Task<List<RoleModulePermission>> RoleModuleMaps();
        Task<List<RoleModulePermission>> GetRMPMaps();
    }
}
