using System.Collections.Generic;
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
        // readonly INpoiWordExportServices _NpoiWordExportServices;
        readonly ITenanServices _tenanServices;
        readonly IRoleTenanServices _roleTenanServices;
        readonly IRoleServices _roleServices;
        readonly IUser _user;
     
        public RoleController(/*INpoiWordExportServices NpoiWordExportServices,*/ITenanServices tenanServices,IRoleTenanServices roleTenanServices,IRoleServices roleServices, IUser user)
        {
            //_NpoiWordExportServices = NpoiWordExportServices;
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
        [AllowAnonymous]
        public async Task<MessageModel<Role>> GetById(int rid)
        {

            var data = new MessageModel<Role> { response = await _roleServices.QueryById(rid) };
            if (data.response != null)
            {
                var allRoleTenans = await _roleTenanServices.Query(d => d.IsDeleted == false);
                var allTenans = await _tenanServices.Query(d => d.Enabled == true);

                var currentUserRoles = allRoleTenans.Where(d => d.RoleId == data.response.Id).Select(d => d.TenanId).ToList();
                data.response.TIDs = currentUserRoles;
                data.response.TenanNames = allTenans.Where(d => currentUserRoles.Contains(d.Id)).Select(d => d.TenanName).ToList();

                data.success = true;
                data.msg = "";
            }
            return data;
        }

        /// <summary>
        /// 根据客户Id获取客户下所有角色
        /// </summary>
        /// <param name="tid"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<MessageModel<List<Role>>> GetByTid(int tid)
        {

            if (tid == 1)
            {
                var roleinfo = await _roleServices.Query(a=>a.IsDeleted==false);

                var data = new MessageModel<List<Role>> { response = roleinfo, msg = "", success = true };
                return data;
            }
            else {
                //先根据关联表获取角色ID，再循环获取角色name
                var roidlist = await _roleTenanServices.Query(x => x.TenanId == tid);

                List<object> roidList = new List<object>();
                foreach (var item in roidlist)
                {
                    roidList.Add(item.RoleId);
                }
                var roleinfo = await _roleServices.QueryByIDs(roidList.ToArray());

                // var data = new MessageModel<List<Role>> { response = await _roleServices.QueryMuchTable(tid),msg="",success=true };
                var data = new MessageModel<List<Role>> { response = roleinfo, msg = "", success = true };
                return data;
            }

        }



        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <param name="key">角色名称</param>
        /// <param name="id">客户ID</param>
        /// <returns></returns>
        // GET: api/User
        [HttpGet]
        public async Task<MessageModel<PageModel<Role>>> Get(int pageindex = 1,int pagesize=50, string key = "",int id=0)
        {
            //这里区分总管理员
            if (id == 1)
            {

                var data = await _roleServices.QueryPage(a => a.IsDeleted != true  && (a.Name != null && a.Name.Contains(key)), pageindex, pagesize, " Id desc ");

                // 这里可以封装到多表查询，此处简单处理
                var allRoleTenans = await _roleTenanServices.Query(d => d.IsDeleted == false);
                var allTenans = await _tenanServices.Query(d => d.Enabled == true);

                var sysUserInfos = data.data;
                foreach (var item in sysUserInfos)
                {
                    var currentUserRoles = allRoleTenans.Where(d => d.RoleId == item.Id).Select(d => d.TenanId).ToList();
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
            else {
                //根据客户id获取客户角色关联信息
                //获取角色id集合
                var roletenan = await _roleTenanServices.Query(a => a.TenanId == id);
                List<int> strList = new List<int>();
                foreach (var item in roletenan)
                {
                    strList.Add(item.RoleId);
                }
                var data = await _roleServices.QueryPage(a => a.IsDeleted != true && strList.Contains(a.Id) && (a.Name != null && a.Name.Contains(key)), pageindex, pagesize, " Id desc ");

                // 这里可以封装到多表查询，此处简单处理
                var allRoleTenans = await _roleTenanServices.Query(d => d.IsDeleted == false);
                var allTenans = await _tenanServices.Query(d => d.Enabled == true);

                var sysUserInfos = data.data;
                foreach (var item in sysUserInfos)
                {
                    var currentUserRoles = allRoleTenans.Where(d => d.RoleId == item.Id).Select(d => d.TenanId).ToList();
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

                //同时更新角色客户关联信息
                //先删除，再新增
                if (data.success)
                {
                    RoleTenan model = new RoleTenan()
                    {
                        RoleId= role.Id
                    };
                    await _roleTenanServices.Delete(a=>a.RoleId== role.Id);
                    foreach (var item in role.TIDs) {
                        model.RoleId = role.Id;
                        model.TenanId = item;
                        model.IsDeleted = false;
                       
                        await _roleTenanServices.Add(model);
                    }
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

                    RoleTenan model = new RoleTenan()
                    {
                        RoleId = id
                    };
                    //await _roleTenanServices.Delete(model);
                    foreach (var item in role.TIDs)
                    {
                        model.RoleId = id;
                        model.TenanId = item;
                        model.IsDeleted = false;
                        await _roleTenanServices.Add(model);
                    }

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
        /// 禁用/启用角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<MessageModel<string>> DeleteOrActivation(int id)
        {

           // bool result = await _NpoiWordExportServices.SaveWordFile("", "AchOrg", 1);

            var data = new MessageModel<string>();
            if (id > 0)
            {
                var userDetail = await _roleServices.QueryById(id);
                userDetail.Enabled = !userDetail.Enabled;
                data.success = await _roleServices.Update(userDetail);
                if (data.success)
                {
                    data.msg = "操作成功";
                    data.response = userDetail?.Id.ObjToString();
                }
            }

            return data;
        }
    }
}
