using System.Linq;
using System.Threading.Tasks;
using Funeral.Core.Common.HttpContextUser;
using Funeral.Core.IRepository;
using Funeral.Core.IServices;
using Funeral.Core.Model;
using Funeral.Core.Model.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Funeral.Core.Controllers
{
    /// <summary>
    /// 角色管理
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Permissions.Name)]
    public class RoleController : ControllerBase
    {
        readonly ITenanServices _tenanServices;
        readonly IRoleTenanServices _roleTenanServices;
        readonly IRoleServices _roleServices;
        readonly IUser _user;
     
        public RoleController(ITenanServices tenanServices,IRoleTenanServices roleTenanServices,IRoleServices roleServices, IUser user)
        {
            _tenanServices = tenanServices;
            _roleTenanServices = roleTenanServices;
            _roleServices = roleServices;
            _user = user;
        }

        /// <summary>
        /// 根据ID获取角色信息
        /// </summary>
        /// <param name="rid"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<MessageModel<Role>> GetById(int rid)
        {

            var data = new MessageModel<Role> { response = await _roleServices.QueryById(rid) };
            if (data.response != null)
            {
                data.success = true;
                data.msg = "";
            }
            return data;
        }



        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <param name="key">角色名称</param>
        /// <returns></returns>
        // GET: api/User
        [HttpGet]
        public async Task<MessageModel<PageModel<Role>>> Get(int pageindex = 1,int pagesize=50, string key = "")
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
            {
                key = "";
            }

            var data = await _roleServices.QueryPage(a => a.IsDeleted != true && (a.Name != null && a.Name.Contains(key)), pageindex, pagesize, " Id desc ");

            // 这里可以封装到多表查询，此处简单处理
            var allRoleTenans = await _roleTenanServices.Query(d => d.IsDeleted == false);
            var allTenans = await _tenanServices.Query(d => d.Enabled == true);

            var sysUserInfos = data.data;
            foreach (var item in sysUserInfos)
            {
                var currentUserRoles = allRoleTenans.Where(d => d.RoleId == item.Id).Select(d => d.RoleId).ToList();
                item.TIDs = currentUserRoles;
                item.TenanNames = allTenans.Where(d => currentUserRoles.Contains(d.Id)).Select(d => d.TenanName).ToList();
            }

            data.data = sysUserInfos;

            return new MessageModel<PageModel<Role>>()
            {
                msg = "获取成功",
                success = data.dataCount >= 0,
                response = data
            };

        }


        /// <summary>
        /// 新增/更新角色
        /// </summary>
        /// <param name="role">角色信息实体</param>
        /// <returns></returns>
        // POST: api/User
        [HttpPost]
        public async Task<MessageModel<string>> Post([FromBody] Role role)
        {
            var data = new MessageModel<string>();

            if (role != null && role.Id > 0)
            {
                //更新
                data.success = await _roleServices.Update(role);
                if (data.success)
                {
                    data.msg = "更新成功";
                    data.response = role?.Id.ObjToString();
                }
            }
            else {
                //新增
                role.CreateId = _user.ID;
                role.CreateBy = _user.Name;

                var id = (await _roleServices.Add(role));
                data.success = id > 0;
                if (data.success)
                {
                    data.response = id.ObjToString();
                    data.msg = "添加成功";
                }
            }
     

            return data;
        }

        /// <summary>
        /// 更新角色
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        // PUT: api/User/5
        [HttpPut]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<MessageModel<string>> Put([FromBody] Role role)
        {
            var data = new MessageModel<string>();
            if (role != null && role.Id > 0)
            {
                data.success = await _roleServices.Update(role);
                if (data.success)
                {
                    data.msg = "更新成功";
                    data.response = role?.Id.ObjToString();
                }
            }

            return data;
        }

        /// <summary>
        /// 禁用角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<MessageModel<string>> Delete(int id)
        {
            var data = new MessageModel<string>();
            if (id > 0)
            {
                var userDetail = await _roleServices.QueryById(id);
                userDetail.Enabled = false;
                data.success = await _roleServices.Update(userDetail);
                if (data.success)
                {
                    data.msg = "禁用成功";
                    data.response = userDetail?.Id.ObjToString();
                }
            }

            return data;
        }
    }
}
