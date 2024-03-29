﻿using System.Threading.Tasks;
using Funeral.Core.IServices;
using Funeral.Core.Model;
using Funeral.Core.Model.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Funeral.Core.Controllers
{
    /// <summary>
    /// 用户角色关系
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    //[Authorize(Permissions.Name)]
    public class UserRoleController : Controller
    {
        readonly IPermissionTenanServices _permissionTenanServices;
        readonly IRoleTenanServices _roleTenanServices;
        readonly ISysUserInfoServices _sysUserInfoServices;
        readonly IUserRoleServices _userRoleServices;
        readonly IRoleServices _roleServices;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sysUserInfoServices"></param>
        /// <param name="userRoleServices"></param>
        /// <param name="roleServices"></param>
        public UserRoleController(IPermissionTenanServices permissionTenanServices, IRoleTenanServices roleTenanServices ,ISysUserInfoServices sysUserInfoServices, IUserRoleServices userRoleServices, IRoleServices roleServices)
        {
            this._permissionTenanServices = permissionTenanServices;
            this._roleTenanServices = roleTenanServices;
            this._sysUserInfoServices = sysUserInfoServices;
            this._userRoleServices = userRoleServices;
            this._roleServices = roleServices;
        }



        /// <summary>
        /// 新建用户
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="loginPwd"></param>
        /// <returns></returns>
        [HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<MessageModel<sysUserInfo>> AddUser(string loginName, string loginPwd)
        {
            return new MessageModel<sysUserInfo>()
            {
                success = true,
                msg = "添加成功",
                response = await _sysUserInfoServices.SaveUserInfo(loginName, loginPwd)
            };
        }

        /// <summary>
        /// 新建角色
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        [HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<MessageModel<Role>> AddRole(string roleName)
        {
            return new MessageModel<Role>()
            {
                success = true,
                msg = "添加成功",
                response = await _roleServices.SaveRole(roleName)
            };
        }

        /// <summary>
        /// 新建用户角色关系
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <param name="rid">角色ID</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<MessageModel<UserRole>> AddUserRole(int uid, int rid)
        {
            return new MessageModel<UserRole>()
            {
                success = true,
                msg = "添加成功",
                response = await _userRoleServices.SaveUserRole(uid, rid)
            };
        }


        /// <summary>
        /// 新建角色客户关系关系
        /// </summary>
        /// <param name="rid">角色ID</param>
        /// <param name="tid">客户ID</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<MessageModel<RoleTenan>> AddRoleTenan(int rid, int tid)
        {
            return new MessageModel<RoleTenan>()
            {
                success = true,
                msg = "添加成功",
                response = await _roleTenanServices.SaveRoleTenan(rid, tid)
            };
        }

        /// <summary>
        /// 新建菜单和客户关联关系
        /// </summary>
        /// <param name="pid">菜单ID</param>
        /// <param name="tid">客户ID</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<MessageModel<PermissionTenan>> AddPermissionTenan(int pid, int tid)
        {
            return new MessageModel<PermissionTenan>()
            {
                success = true,
                msg = "添加成功",
                response = await _permissionTenanServices.SavePermissionTenan(pid, tid)
            };
        }





    }
}
