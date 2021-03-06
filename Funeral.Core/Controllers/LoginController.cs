﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Funeral.Core.AuthHelper;
using Funeral.Core.AuthHelper.OverWrite;
using Funeral.Core.Common.Helper;
using Funeral.Core.IServices;
using Funeral.Core.Model;
using Funeral.Core.Model.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Funeral.Core.Controllers
{
    /// <summary>
    /// 登录管理【无权限】
    /// </summary>
    [Produces("application/json")]
    [Route("api/Login")]
    [AllowAnonymous]
    public class LoginController : Controller
    {
        readonly IRoleTenanServices _roleTenanServices;
        readonly ISysUserInfoServices _sysUserInfoServices;
        readonly IUserRoleServices _userRoleServices;
        readonly PermissionRequirement _requirement;
        private readonly IRoleModulePermissionServices _roleModulePermissionServices;


        /// <summary>
        /// 构造函数注入
        /// </summary>
        /// <param name="sysUserInfoServices"></param>
        /// <param name="userRoleServices"></param>
        /// <param name="requirement"></param>
        /// <param name="roleModulePermissionServices"></param>
        public LoginController(IRoleTenanServices roleTenanServices ,ISysUserInfoServices sysUserInfoServices, IUserRoleServices userRoleServices, PermissionRequirement requirement, IRoleModulePermissionServices roleModulePermissionServices)
        {
            this._roleTenanServices = roleTenanServices;
            this._sysUserInfoServices = sysUserInfoServices;
            this._userRoleServices = userRoleServices;
            _requirement = requirement;
            _roleModulePermissionServices = roleModulePermissionServices;
        }


        #region 获取token的第1种方法
        /// <summary>
        /// 获取JWT的方法1
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Token")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<MessageModel<string>> GetJwtStr(string name, string pass)
        {
            string jwtStr = string.Empty;
            bool suc = false;
            //这里就是用户登陆以后，通过数据库去调取数据，分配权限的操作

            var user = await _sysUserInfoServices.GetUserRoleNameStr(name, MD5Helper.MD5Encrypt32(pass));
            if (user != null)
            {

                TokenModelJwt tokenModel = new TokenModelJwt { Uid = 1, Role = user };

                jwtStr = JwtHelper.IssueJwt(tokenModel);
                suc = true;
            }
            else
            {
                jwtStr = "login fail!!!";
            }

            return new MessageModel<string>()
            {
                success = suc,
                msg = suc ? "获取成功" : "获取失败",
                response = jwtStr
            };
        }


        /// <summary>
        /// 获取JWT的方法2：给Nuxt提供
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetTokenNuxt")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public MessageModel<string> GetJwtStrForNuxt(string name, string pass)
        {
            string jwtStr = string.Empty;
            bool suc = false;
            //这里就是用户登陆以后，通过数据库去调取数据，分配权限的操作
            //这里直接写死了
            if (name == "admins" && pass == "admins")
            {
                TokenModelJwt tokenModel = new TokenModelJwt
                {
                    Uid = 1,
                    Role = "Admin"
                };

                jwtStr = JwtHelper.IssueJwt(tokenModel);
                suc = true;
            }
            else
            {
                jwtStr = "login fail!!!";
            }
            var result = new
            {
                data = new { success = suc, token = jwtStr }
            };

            return new MessageModel<string>()
            {
                success = suc,
                msg = suc ? "获取成功" : "获取失败",
                response = jwtStr
            };
        }
        #endregion


        /// <summary>
        /// 获取JWT的方法3：整个系统主要方法
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("JWTToken3.0")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<MessageModel<TokenInfoViewModel>> GetJwtToken3(string name = "", string pass = "")
        {
            string jwtStr = string.Empty;

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(pass))
            {
                return new MessageModel<TokenInfoViewModel>()
                {
                    success = false,
                    msg = "用户名或密码不能为空",
                };
            }

            pass = MD5Helper.MD5Encrypt32(pass);

            var user = await _sysUserInfoServices.Query(d => d.uLoginName == name && d.uLoginPWD == pass && d.tdIsDelete == false);
            if (user.Count > 0)
            {
                //判断所选部门是否为空并且判断部门和用户是否匹配



                var userRoles = await _sysUserInfoServices.GetUserRoleNameStr(name, pass);
                //如果是基于用户的授权策略，这里要添加用户;如果是基于角色的授权策略，这里要添加角色
                var claims = new List<Claim> {
                    new Claim(ClaimTypes.Name, name),
                    new Claim(JwtRegisteredClaimNames.Jti, user.FirstOrDefault().uID.ToString()),
                    new Claim(ClaimTypes.Expiration, DateTime.Now.AddSeconds(_requirement.Expiration.TotalSeconds).ToString()) };
                claims.AddRange(userRoles.Split(',').Select(s => new Claim(ClaimTypes.Role, s)));


                // ids4和jwt切换
                // jwt
                if (!Permissions.IsUseIds4)
                {
                    var data = await _roleModulePermissionServices.RoleModuleMaps();
                    var list = (from item in data
                                where item.IsDeleted == false
                                orderby item.Id
                                select new PermissionItem
                                {
                                    Url = item.Module?.LinkUrl,
                                    Role = item.Role?.Name.ObjToString(),
                                }).ToList();

                    _requirement.Permissions = list;
                }

                var token = JwtToken.BuildJwtToken(claims.ToArray(), _requirement);
                return new MessageModel<TokenInfoViewModel>()
                {
                    success = true,
                    msg = "获取成功",
                    response = token
                };
            }
            else
            {
                return new MessageModel<TokenInfoViewModel>()
                {
                    success = false,
                    msg = "认证失败",
                };
            }
        }


        /// <summary>
        /// 获取JWT的方法：整个系统主要方法（加入部门判断）
        /// </summary>
        /// <param name="userNameOrEmailAddress">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="tenanId">客户ID</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Login")]
        public async Task<MessageModel<TokenInfoViewModel>> GetJwtToken3(string userNameOrEmailAddress = "", string password = "",string tenanId = "")
        {
            //using (MiniProfiler.Current.Step("进入方法"))
            //{

            //    using (MiniProfiler.Current.CustomTiming("HTTP", "GET " + "http://localhost:8081/?userNameOrEmailAddress=admin&password=123qwe&tenanId=1"))
            //    {
            //        var client = new WebClient();
            //        var reply = client.DownloadString("http://localhost:8081/?userNameOrEmailAddress=admin&password=123qwe&tenanId=1");
            //    }
            //}
                string index = "";
            string jwtStr = string.Empty;
            if (string.IsNullOrEmpty(userNameOrEmailAddress) || string.IsNullOrEmpty(password))
            {
                return new MessageModel<TokenInfoViewModel>()
                {
                    success = false,
                    msg = "用户名或密码不能为空",
                };
            }
            if (string.IsNullOrEmpty(tenanId)) {
                return new MessageModel<TokenInfoViewModel>()
                {
                    success = false,
                    msg = "请选择平台客户",
                };
            }

            password = MD5Helper.MD5Encrypt32(password);
            var userid = 0;
            var roleid = 0;
            var tenanid = 0;
            var user = await _sysUserInfoServices.Query(d => d.uLoginName == userNameOrEmailAddress && d.uLoginPWD == password && d.tdIsDelete == false);
            int ifresult = 0;
            if (user.Count > 0)
            {
                //判断所选部门是否为空并且判断部门和用户是否匹配

                foreach (var item in user) {
                     userid = item.uID;
                     roleid = await _userRoleServices.GetRoleIdByUid(item.uID);//获取角色ID
                     tenanid = await _roleTenanServices.GetTenanIdByRid(roleid);//获取部门ID

                    if (tenanid != tenanId.ObjToInt())
                    {
                      
                        continue;
                    }
                    else {
                        ifresult = 1;
                        break;
                    }
                }

                if (ifresult<=0) {
                    return new MessageModel<TokenInfoViewModel>()
                    {
                        success = false,
                        msg = "该平台客户下无此用户信息",
                    };
                }
            

                var userRoles = await _sysUserInfoServices.GetUserRoleNameStr(userNameOrEmailAddress, password);
                //如果是基于用户的授权策略，这里要添加用户;如果是基于角色的授权策略，这里要添加角色
                var claims = new List<Claim> {
                    new Claim(ClaimTypes.Name, userNameOrEmailAddress),
                    new Claim(JwtRegisteredClaimNames.Jti, userid.ToString()),//用户信息
                    new Claim(ClaimTypes.MobilePhone, tenanid.ToString()),//客户信息
                    new Claim(ClaimTypes.Expiration, DateTime.Now.AddSeconds(_requirement.Expiration.TotalSeconds).ToString()) };//过期时间
                claims.AddRange(userRoles.Split(',').Select(s => new Claim(ClaimTypes.Role, s)));

                // ids4和jwt切换
                // jwt
                if (!Permissions.IsUseIds4)
                {
                    var data = await _roleModulePermissionServices.RoleModuleMaps();
                    var list = (from item in data
                                where item.IsDeleted == false
                                orderby item.Id
                                select new PermissionItem
                                {
                                    Url = item.Module?.LinkUrl,
                                    Role = item.Role?.Name.ObjToString(),
                                }).ToList();

                    _requirement.Permissions = list;
                }

                var token = JwtToken.BuildJwtToken(claims.ToArray(), _requirement);
                token.uid = userid;
                token.rid = roleid;
                token.tid = tenanid;

                return new MessageModel<TokenInfoViewModel>()
                {
                    success = true,
                    msg = "获取成功",
                    response = token
                };
            }
            else
            {
                return new MessageModel<TokenInfoViewModel>()
                {
                    success = false,
                    msg = "认证失败",
                };
            }
        }


        /// <summary>
        /// 请求刷新Token（以旧换新）
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("RefreshToken")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<MessageModel<TokenInfoViewModel>> RefreshToken(string token = "")
        {
            string jwtStr = string.Empty;

            if (string.IsNullOrEmpty(token))
            {
                return new MessageModel<TokenInfoViewModel>()
                {
                    success = false,
                    msg = "token无效，请重新登录！",
                };
            }
            var tokenModel = JwtHelper.SerializeJwt(token);
            if (tokenModel != null && tokenModel.Uid > 0)
            {
                var user = await _sysUserInfoServices.QueryById(tokenModel.Uid);
                if (user != null)
                {
                    var userRoles = await _sysUserInfoServices.GetUserRoleNameStr(user.uLoginName, user.uLoginPWD);
                    //如果是基于用户的授权策略，这里要添加用户;如果是基于角色的授权策略，这里要添加角色
                    var claims = new List<Claim> {
                    new Claim(ClaimTypes.Name, user.uLoginName),
                    new Claim(JwtRegisteredClaimNames.Jti, tokenModel.Uid.ObjToString()),
                    new Claim(ClaimTypes.Expiration, DateTime.Now.AddSeconds(_requirement.Expiration.TotalSeconds).ToString()) };
                    claims.AddRange(userRoles.Split(',').Select(s => new Claim(ClaimTypes.Role, s)));

                    //用户标识
                    var identity = new ClaimsIdentity(JwtBearerDefaults.AuthenticationScheme);
                    identity.AddClaims(claims);

                    var refreshToken = JwtToken.BuildJwtToken(claims.ToArray(), _requirement);
                    return new MessageModel<TokenInfoViewModel>()
                    {
                        success = true,
                        msg = "获取成功",
                        response = refreshToken
                    };
                }
            }

            return new MessageModel<TokenInfoViewModel>()
            {
                success = false,
                msg = "认证失败！",
            };
        }

        /// <summary>
        /// 获取JWT的方法4：给 JSONP 测试
        /// </summary>
        /// <param name="callBack"></param>
        /// <param name="id"></param>
        /// <param name="sub"></param>
        /// <param name="expiresSliding"></param>
        /// <param name="expiresAbsoulute"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("jsonp")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public void Getjsonp(string callBack, long id = 1, string sub = "Admin", int expiresSliding = 30, int expiresAbsoulute = 30)
        {
            TokenModelJwt tokenModel = new TokenModelJwt
            {
                Uid = id,
                Role = sub
            };

            string jwtStr = JwtHelper.IssueJwt(tokenModel);

            string response = string.Format("\"value\":\"{0}\"", jwtStr);
            string call = callBack + "({" + response + "})";
            Response.WriteAsync(call);
        }


        /// <summary>
        ///  MD5 加密字符串
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Md5Password")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public string Md5Password(string password = "")
        {
            return MD5Helper.MD5Encrypt32(password);
        }
    }
}