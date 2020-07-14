using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Funeral.Core.Common.Helper;
using Funeral.Core.Common.HttpContextUser;
using Funeral.Core.IServices;
using Funeral.Core.Model;
using Funeral.Core.Model.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Funeral.Core.Controllers
{

    /// <summary>
    /// 客户管理
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]

    public class TenanController : ControllerBase
    {
        readonly IUserRoleServices _userRoleServices;
        readonly ISysUserInfoServices _sysUserInfoServices;
        readonly IRoleTenanServices  _roleTenanServices;
        readonly IRoleServices _roleServices;
        readonly ITenanServices _tenanServices;
        readonly IUser _user;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="tenanServices"></param>
        public TenanController(IUserRoleServices userRoleServices, ISysUserInfoServices sysUserInfoServices, IRoleTenanServices roleTenanServices, IRoleServices roleServices, ITenanServices tenanServices, IUser user)
        {
            _userRoleServices = userRoleServices;
            _sysUserInfoServices = sysUserInfoServices;
            _roleTenanServices = roleTenanServices;
            _roleServices = roleServices;
            _tenanServices = tenanServices;
            _user = user;
        }


        /// <summary>
        /// 获取客户列表
        /// </summary>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <param name="key">客户名称</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Get")]
        [AllowAnonymous]
        //[AllowAnonymous]
        //[Authorize(Permissions.Name)]
        //[ApiExplorerSettings(IgnoreApi = true)]
        public async Task<MessageModel<PageModel<Tenan>>> Get(int pageindex = 1,int pagesize=50, string key = "")
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
            {
                key = "";
            }

            var data = await _tenanServices.QueryPage(a => (a.TenanName != null && a.TenanName.Contains(key)), pageindex, pagesize, " Id desc ");

            return new MessageModel<PageModel<Tenan>>()
            {
                msg = "获取成功",
                success = data.dataCount >= 0,
                response = data
            };

        }


        /// <summary>
        /// 获取所有客户信息（登录用）
        /// </summary>
        /// <returns></returns>
        // GET: api/Topic
        [HttpGet]
        [Route("GetTenanAll")]
        [AllowAnonymous]
        public async Task<MessageModel<List<Tenan>>> GetAll ()
        {
            var data = new MessageModel<List<Tenan>> { response = await _tenanServices.GetAll() };
            if (data.response != null)
            {
                data.success = true;
                data.msg = "";
            }
            return data;
        }


        /// <summary>
        /// 新增/更新客户
        /// </summary>
        /// <param name="tenan">客户信息实体</param>
        /// <returns></returns>
        [HttpPost]
        [Route("Post")]
        [AllowAnonymous]
        public async Task<MessageModel<string>> Post([FromBody] Tenan tenan)
        {
            var data = new MessageModel<string>();

            if (tenan != null && tenan.Id > 0)
            {
                //更新
                tenan.ModifyBy = _user.ID.ToString();
                tenan.ModifyBy = _user.Name;
                data.success = await _tenanServices.Update(tenan);
                if (data.success)
                {
                    data.msg = "更新成功";
                    data.response = tenan?.Id.ObjToString();
                }
            }
            else {
                //新增
                tenan.CreateBy = _user.ID.ToString();
                tenan.CreateBy = _user.Name;
                var id = (await _tenanServices.Add(tenan));
                data.success = id > 0;
                if (data.success)
                {
                    data.response = id.ObjToString();
                    data.msg = "添加成功";

                    #region 同步新增一个超级管理员
                    Role role = new Role() { };
                    role.Name = tenan.TenanName+ "管理员角色";
                    role.Enabled = true;
                    role.Description = "管理员角色";
                    role.TIDs = id;
                    role.CreateId = _user.ID;
                    role.CreateBy = _user.Name;

                    id = (await _roleServices.Add(role));
                    data.success = id > 0;
                    if (data.success)
                    {
                        RoleTenan model = new RoleTenan()
                        {
                            RoleId = id
                        };
                        model.RoleId = id;
                        model.TenanId = role.TIDs;
                        model.IsDeleted = false;
                        await _roleTenanServices.Add(model);


                        List<int> list = new List<int>();
                        list.Add(id);
                        #region 添加默认用户

                        sysUserInfo user = new sysUserInfo() { };
                        user.uLoginName = "admin";
                        user.uLoginPWD = MD5Helper.MD5Encrypt32("admin");
                        user.uRealName = "管理员";
                        user.birth = DateTime.Now;
                        user.Enabled = true;
                        user.RIDs = list;

                        id = (await _sysUserInfoServices.Add(user));
                        data.success = id > 0;
                        if (data.success) {
                            var userRolsAdd = new List<UserRole>();
                            user.RIDs.ForEach(rid =>
                            {
                                userRolsAdd.Add(new UserRole(id, rid));
                            });
                            await _userRoleServices.Add(userRolsAdd);
                        }
                        #endregion


                        
                    }
                    #endregion
                  
                }
            }
            return data;
        }


        /// <summary>
        /// 禁用/启用客户
        /// </summary>
        /// <param name="id">客户ID</param>
        /// <returns></returns>
        [HttpGet]
        [Route("DeleteOrActivation")]
        [AllowAnonymous]
        public async Task<MessageModel<string>> DeleteOrActivation(int id)
        {
            var data = new MessageModel<string>();
            if (id > 0)
            {
                var userDetail = await _tenanServices.QueryById(id);
                userDetail.Enabled = !userDetail.Enabled;
                data.success = await _tenanServices.Update(userDetail);
                if (data.success)
                {
                    data.msg = "操作成功";
                    data.response = userDetail?.Id.ObjToString();
                }
                else {
                    data.msg = "操作失败";
                    data.response = userDetail?.Id.ObjToString();
                }
            }

            return data;
        }

        /// <summary>
        /// 根据ID获取客户信息
        /// </summary>
        /// <param name="tid"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetById")]
        [AllowAnonymous]
        public async Task<MessageModel<Tenan>> GetById(int tid) {

            var data = new MessageModel<Tenan> { response = await _tenanServices.QueryById(tid) };
            if (data.response != null)
            {
                data.success = true;
                data.msg = "";
            }
            return data;
        }

    }
}
