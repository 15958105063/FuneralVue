using Funeral.Core.IServices.BASE;
using Funeral.Core.Model.Models;
using System.Threading.Tasks;

namespace Funeral.Core.IServices
{
    /// <summary>
    /// IPermissionTenanServices
    /// </summary>	
    public interface IPermissionTenanServices : IBaseServices<PermissionTenan>
	{
        Task<PermissionTenan> SavePermissionTenan(int rid, int tid);
        Task<int> GetTenanIdByPid(int pid);
    }
}

