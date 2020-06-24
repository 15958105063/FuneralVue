using Funeral.Core.IServices;
using Funeral.Core.IRepository;
using Funeral.Core.Services.BASE;
using Funeral.Core.Model.Models;
using System.Threading.Tasks;
using System.Linq;
using Funeral.Core.Common;
using Funeral.Core.IRepository.UnitOfWork;

namespace Funeral.Core.Services
{	
	/// <summary>
	/// RoleServices
	/// </summary>	
	public class RoleServices : BaseServices<Role>, IRoleServices
    {
        private readonly IUnitOfWork _unitOfWork;
            IRoleRepository _dal;
        public RoleServices(IUnitOfWork unitOfWork,IRoleRepository dal)
        {
            this._unitOfWork = unitOfWork;
            this._dal = dal;
            base.BaseDal = dal;
        }
       /// <summary>
       /// 
       /// </summary>
       /// <param name="roleName"></param>
       /// <returns></returns>
        public async Task<Role> SaveRole(string roleName)
        {
            Role role = new Role(roleName);
            Role model = new Role();
            var userList = await base.Query(a => a.Name == role.Name && a.Enabled);
            if (userList.Count > 0)
            {
                model = userList.FirstOrDefault();
            }
            else
            {
                var id = await base.Add(role);
                model = await base.QueryById(id);
            }

            return model;

        }

        [Caching(AbsoluteExpiration = 30)]
        public async Task<string> GetRoleNameByRid(int rid)
        {
            return ((await base.QueryById(rid))?.Name);
        }
    }
}
