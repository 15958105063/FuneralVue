using Funeral.Core.IServices;
using Funeral.Core.FrameWork.IRepository;
using Funeral.Core.Services.BASE;
using Funeral.Core.Model.Models;
using System.Threading.Tasks;
using System.Linq;
using Funeral.Core.Common;
using Funeral.Core;
using Funeral.Core.Common.HttpContextUser;
using Funeral.Core.IRepository;

namespace Funeral.Core.Services
{	
	/// <summary>
	/// UserRoleServices
	/// </summary>	
	public class UserRoleServices : BaseServices<UserRole>, IUserRoleServices
    {
        readonly IUser _user;
        IUserRoleRepository _dal;
        IRoleTenanRepository _roleTenanRepository;
        ITenanRepository _tenanRepository;

        public UserRoleServices(ITenanRepository tenanRepository, IRoleTenanRepository roleTenanRepository, IUser user, IUserRoleRepository dal)
        {
            _tenanRepository = tenanRepository;
            _roleTenanRepository = roleTenanRepository;
            _user = user;
            this._dal = dal;
            base.BaseDal = dal;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="rid"></param>
        /// <returns></returns>
        public async Task<UserRole> SaveUserRole(int uid, int rid)
        {
            UserRole userRole = new UserRole(uid, rid);

            UserRole model = new UserRole();
            var userList = await base.Query(a => a.UserId == userRole.UserId && a.RoleId == userRole.RoleId);
            if (userList.Count > 0)
            {
                model = userList.FirstOrDefault();
            }
            else
            {
                var id = await base.Add(userRole);
                model = await base.QueryById(id);
            }
            return model;
        }

        public async Task<int> GetRoleIdByUid(int uid)
        {
            return ((await base.Query(d => d.UserId == uid)).OrderByDescending(d => d.Id).LastOrDefault()?.RoleId).ObjToInt();
        }


        /// <summary>
        /// 获取当前登录用户的所属客户
        /// </summary>
        /// <returns></returns>
        public async Task<Tenan> GetLoginTenan()
        {
            int roleid = ((await base.Query(a => a.UserId == _user.ID)).OrderByDescending(a => a.Id).LastOrDefault()?.RoleId).ObjToInt();
            int tenanid= ((await  _roleTenanRepository.Query(a=>a.RoleId== roleid)).OrderByDescending(a => a.TenanId).LastOrDefault()?.TenanId).ObjToInt();
            return (await _tenanRepository.Query(a=>a.Id== tenanid)).SingleOrDefault();
        }

    }
}
