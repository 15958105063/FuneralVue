using Funeral.Core.IServices.BASE;
using Funeral.Core.Model.Models;
using System.Threading.Tasks;

namespace Funeral.Core.IServices
{
    /// <summary>
    /// IRoleTenanServices
    /// </summary>	
    public interface IRoleTenanServices : IBaseServices<RoleTenan>
	{
        Task<RoleTenan> SaveRoleTenan(int rid, int tid);
        Task<int> GetTenanIdByRid(int rid);
    }
}

