using Funeral.Core.IServices.BASE;
using Funeral.Core.Model.Models;
using System.Threading.Tasks;

namespace Funeral.Core.IServices
{	
	/// <summary>
	/// UserRoleServices
	/// </summary>	
    public interface IUserRoleServices :IBaseServices<UserRole>
	{

        Task<UserRole> SaveUserRole(int uid, int rid);
        Task<int> GetRoleIdByUid(int uid);
    }
}

