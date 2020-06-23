    

using Funeral.Core.IServices.BASE;
using Funeral.Core.Model.Models;
using System.Threading.Tasks;

namespace Funeral.Core.IServices
{	
	/// <summary>
	/// sysUserInfoServices
	/// </summary>	
    public interface ISysUserInfoServices :IBaseServices<sysUserInfo>
	{
        Task<sysUserInfo> SaveUserInfo(string loginName, string loginPwd);
        Task<string> GetUserRoleNameStr(string loginName, string loginPwd);
    }
}
