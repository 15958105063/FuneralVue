using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Funeral.Core.AuthHelper.OverWrite;
using Funeral.Core.Common.Helper;
using Funeral.Core.Common.HttpContextUser;
using Funeral.Core.IRepository.UnitOfWork;
using Funeral.Core.IServices;
using Funeral.Core.Model;
using Funeral.Core.Model.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Funeral.Core.Controllers
{
    /// <summary>
    /// 用户管理
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    //[Authorize(Permissions.Name)]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        readonly ISysUserInfoServices _sysUserInfoServices;
        readonly IUserRoleServices _userRoleServices;
        readonly IRoleServices _roleServices;
        private readonly IUser _user;
        private readonly ILogger<UserController> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="sysUserInfoServices"></param>
        /// <param name="userRoleServices"></param>
        /// <param name="roleServices"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public UserController(IUnitOfWork unitOfWork, ISysUserInfoServices sysUserInfoServices, IUserRoleServices userRoleServices, IRoleServices roleServices, IUser user, ILogger<UserController> logger)
        {
            _unitOfWork = unitOfWork;
            _sysUserInfoServices = sysUserInfoServices;
            _userRoleServices = userRoleServices;
            _roleServices = roleServices;
            _user = user;
            _logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        public string GetUserId() {
            return _user.ID.ToString() + "---" + _user.Name;
        }


        /// <summary>
        /// 根据ID获取角色信息
        /// </summary>
        /// <param name="rid"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<MessageModel<sysUserInfo>> GetById(int uid)
        {


            var data = await _sysUserInfoServices.QueryById(uid);


            #region MyRegion

            // 这里可以封装到多表查询，此处简单处理
            var allUserRoles = await _userRoleServices.Query(d => d.IsDeleted == false);
            var allRoles = await _roleServices.Query(d => d.IsDeleted == false);


                var currentUserRoles = allUserRoles.Where(d => d.UserId == data.uID).Select(d => d.RoleId).ToList();
            data.RIDs = currentUserRoles;
            data.RoleNames = allRoles.Where(d => currentUserRoles.Contains(d.Id)).Select(d => d.Name).ToList();

           // data.uLoginPWD= MD5Helper.Md5Decrypt(data.uLoginPWD);

            #endregion

            return new MessageModel<sysUserInfo>()
            {
                msg = "获取成功",
                success = true,
                response = data
            };


        }




        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <param name="key">用户姓名</param>
        /// <param name="id">角色ID</param>
        /// <returns></returns>
        // GET: api/User
        [HttpGet]
        [AllowAnonymous]
        public async Task<MessageModel<PageModel<sysUserInfo>>> Get(int pageindex = 1, int pagesize = 50, string key = "", int id = 0)
        {
            //这里区分总管理员
            if (id == 1)
            {
                var data = await _sysUserInfoServices.QueryPage(a =>  a.tdIsDelete != true && a.uStatus >= 0 && ((a.uLoginName != null && a.uLoginName.Contains(key)) || (a.uRealName != null && a.uRealName.Contains(key))), pageindex, pagesize, " uID desc ");


                #region MyRegion

                // 这里可以封装到多表查询，此处简单处理
                var allUserRoles = await _userRoleServices.Query(d => d.IsDeleted == false);
                var allRoles = await _roleServices.Query(d => d.IsDeleted == false);

                var sysUserInfos = data.data;
                foreach (var item in sysUserInfos)
                {
                    var currentUserRoles = allUserRoles.Where(d => d.UserId == item.uID).Select(d => d.RoleId).ToList();
                    item.RIDs = currentUserRoles;
                    item.RoleNames = allRoles.Where(d => currentUserRoles.Contains(d.Id)).Select(d => d.Name).ToList();
                }

                data.data = sysUserInfos;
                #endregion

                return new MessageModel<PageModel<sysUserInfo>>()
                {
                    msg = "获取成功",
                    success = data.dataCount >= 0,
                    response = data
                };
            }
            else {
                //根据角色，获取角色下设置的用户
                //获取用户id集合
                var roletenan = await _userRoleServices.Query(a => a.RoleId == id);

                List<int> strList = new List<int>();
                foreach (var item in roletenan)
                {
                    strList.Add(item.UserId);
                }
                var data = await _sysUserInfoServices.QueryPage(a => strList.Contains(a.uID) && a.tdIsDelete != true && a.uStatus >= 0 && ((a.uLoginName != null && a.uLoginName.Contains(key)) || (a.uRealName != null && a.uRealName.Contains(key))), pageindex, pagesize, " uID desc ");


                #region MyRegion

                // 这里可以封装到多表查询，此处简单处理
                var allUserRoles = await _userRoleServices.Query(d => d.IsDeleted == false);
                var allRoles = await _roleServices.Query(d => d.IsDeleted == false);

                var sysUserInfos = data.data;
                foreach (var item in sysUserInfos)
                {
                    var currentUserRoles = allUserRoles.Where(d => d.UserId == item.uID).Select(d => d.RoleId).ToList();
                    item.RIDs = currentUserRoles;
                    item.RoleNames = allRoles.Where(d => currentUserRoles.Contains(d.Id)).Select(d => d.Name).ToList();
                }

                data.data = sysUserInfos;
                #endregion

                return new MessageModel<PageModel<sysUserInfo>>()
                {
                    msg = "获取成功",
                    success = data.dataCount >= 0,
                    response = data
                };
            }

      

        }

 

        // GET: api/User/5
        /// <summary>
        /// 获取用户详情根据token
        /// 【无权限】
        /// </summary>
        /// <param name="token">令牌</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<MessageModel<sysUserInfo>> GetInfoByToken(string token)
        {
            var data = new MessageModel<sysUserInfo>();
            if (!string.IsNullOrEmpty(token))
            {
                var tokenModel = JwtHelper.SerializeJwt(token);
                if (tokenModel != null && tokenModel.Uid > 0)
                {
                    var userinfo = await _sysUserInfoServices.QueryById(tokenModel.Uid);
                    if (userinfo != null)
                    {
                        data.response = userinfo;
                        data.success = true;
                        data.msg = "获取成功";
                    }
                }

            }
            return data;
        }

        /// <summary>
        /// 新增/更新用户
        /// </summary>
        /// <param name="sysUserInfo"></param>
        /// <returns></returns>
        // POST: api/User
        [HttpPost]
        [AllowAnonymous]
        public async Task<MessageModel<string>> Post([FromBody] sysUserInfo sysUserInfo)
        {
            var data = new MessageModel<string>();

            sysUserInfo.Enabled = true;
            
            if (sysUserInfo != null && sysUserInfo.uID > 0) {
                //更新
                if (sysUserInfo.RIDs.Count > 0)
                {
                    // 无论 Update Or Add , 先删除当前用户的全部 U_R 关系
                    var usreroles = (await _userRoleServices.Query(d => d.UserId == sysUserInfo.uID)).Select(d => d.Id.ToString()).ToArray();
                    if (usreroles.Count() > 0)
                    {
                        var isAllDeleted = await _userRoleServices.DeleteByIds(usreroles);
                    }

                    // 然后再执行添加操作
                    var userRolsAdd = new List<UserRole>();
                    sysUserInfo.RIDs.ForEach(rid =>
                    {
                        userRolsAdd.Add(new UserRole(sysUserInfo.uID, rid));
                    });

                    await _userRoleServices.Add(userRolsAdd);
                }

                data.success = await _sysUserInfoServices.Update(sysUserInfo);

                if (data.success)
                {
                    data.msg = "更新成功";
                    data.response = sysUserInfo?.uID.ObjToString();
                }


            }
            else {

                //var userinfo = (await _sysUserInfoServices.Query(a=>a.uLoginName== sysUserInfo.uLoginName)).FirstOrDefault();
                ////先判断是否已经存在该用户
                //if (userinfo != null)
                //{
                //    data.success = false;
                //    data.response = "";
                //    data.msg = "该登录名系统已存在，请重新填写";
                //}
                //else {
                    //新增
                    sysUserInfo.uLoginPWD = MD5Helper.MD5Encrypt32(sysUserInfo.uLoginPWD);
                    sysUserInfo.uRemark = _user.Name;

                    var id = await _sysUserInfoServices.Add(sysUserInfo);
                    data.success = id > 0;
                    if (data.success)
                    {

                        if (sysUserInfo.RIDs.Count > 0)
                        {
                            // 无论 Update Or Add , 先删除当前用户的全部 U_R 关系
                            var usreroles = (await _userRoleServices.Query(d => d.UserId == id)).Select(d => d.Id.ToString()).ToArray();
                            if (usreroles.Count() > 0)
                            {
                                var isAllDeleted = await _userRoleServices.DeleteByIds(usreroles);
                            }

                            // 然后再执行添加操作
                            var userRolsAdd = new List<UserRole>();
                            sysUserInfo.RIDs.ForEach(rid =>
                            {
                                userRolsAdd.Add(new UserRole(id, rid));
                            });

                            await _userRoleServices.Add(userRolsAdd);
                        }


                        data.response = id.ObjToString();
                        data.msg = "添加成功";
                    }
                //}
              
            }
            return data;
        }

        /// <summary>
        /// 更新用户与角色
        /// </summary>
        /// <param name="sysUserInfo"></param>
        /// <returns></returns>
        // PUT: api/User/5
        [HttpPut]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<MessageModel<string>> Put([FromBody] sysUserInfo sysUserInfo)
        {
            // 这里使用事务处理

            var data = new MessageModel<string>();
            try
            {
                _unitOfWork.BeginTran();

                if (sysUserInfo != null && sysUserInfo.uID > 0)
                {
                    if (sysUserInfo.RIDs.Count > 0)
                    {
                        // 无论 Update Or Add , 先删除当前用户的全部 U_R 关系
                        var usreroles = (await _userRoleServices.Query(d => d.UserId == sysUserInfo.uID)).Select(d => d.Id.ToString()).ToArray();
                        if (usreroles.Count() > 0)
                        {
                            var isAllDeleted = await _userRoleServices.DeleteByIds(usreroles);
                        }

                        // 然后再执行添加操作
                        var userRolsAdd = new List<UserRole>();
                        sysUserInfo.RIDs.ForEach(rid =>
                       {
                           userRolsAdd.Add(new UserRole(sysUserInfo.uID, rid));
                       });

                        await _userRoleServices.Add(userRolsAdd);

                    }

                    data.success = await _sysUserInfoServices.Update(sysUserInfo);

                    _unitOfWork.CommitTran();

                    if (data.success)
                    {
                        data.msg = "更新成功";
                        data.response = sysUserInfo?.uID.ObjToString();
                    }
                }
            }
            catch (Exception e)
            {
                _unitOfWork.RollbackTran();
                _logger.LogError(e, e.Message);
            }
            return data;
        }

        /// <summary>
        /// 禁用/启用用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE: api/ApiWithActions/5
        [HttpGet]
        [AllowAnonymous]
        public async Task<MessageModel<string>> DeleteOrActivation(int id)
        {
            var data = new MessageModel<string>();
            if (id > 0)
            {
                var userDetail = await _sysUserInfoServices.QueryById(id);
                userDetail.Enabled = !userDetail.Enabled;
                data.success = await _sysUserInfoServices.Update(userDetail);
                if (data.success)
                {
                    data.msg = "删除成功";
                    data.response = userDetail?.uID.ObjToString();
                }
            }
            return data;
        }
    }
}
