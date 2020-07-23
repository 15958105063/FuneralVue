using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Funeral.Core.AuthHelper;
using Funeral.Core.AuthHelper.OverWrite;
using Funeral.Core.Common;
using Funeral.Core.Common.Helper;
using Funeral.Core.Common.HttpContextUser;
using Funeral.Core.IServices;
using Funeral.Core.IServices.ES;
using Funeral.Core.Model;
using Funeral.Core.Model.Models;
using Funeral.Core.SwaggerHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Funeral.Core.SwaggerHelper.CustomApiVersion;

namespace Funeral.Core.Controllers
{
    /// <summary>
    /// 菜单以及API权限管理
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    //[Authorize(Permissions.Name)]
    public class PermissionController : ControllerBase
    {
        readonly IESSever _iESSever;
        readonly IPermissionTenanServices _permissionTenanServices;
        readonly ITenanServices _tenanServices;
        readonly IPermissionServices _permissionServices;
        readonly IModuleServices _moduleServices;
        readonly IRoleModulePermissionServices _roleModulePermissionServices;
        readonly IUserRoleServices _userRoleServices;
        readonly IHttpContextAccessor _httpContext;
        readonly IUser _user;
        private readonly PermissionRequirement _requirement;
        private readonly IRedisCacheManager _redisCacheManager;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="permissionServices"></param>
        /// <param name="moduleServices"></param>
        /// <param name="roleModulePermissionServices"></param>
        /// <param name="userRoleServices"></param>
        /// <param name="httpContext"></param>
        /// <param name="user"></param>
        /// <param name="requirement"></param>
        public PermissionController(IESSever eSSever, IRedisCacheManager redisCacheManager, IPermissionTenanServices permissionTenanServices, ITenanServices tenanServices, IPermissionServices permissionServices, IModuleServices moduleServices, IRoleModulePermissionServices roleModulePermissionServices, IUserRoleServices userRoleServices, IHttpContextAccessor httpContext, IUser user, PermissionRequirement requirement)
        {
            _iESSever = eSSever;
            _permissionTenanServices = permissionTenanServices;
            _tenanServices = tenanServices;
            _permissionServices = permissionServices;
            _moduleServices = moduleServices;
            _roleModulePermissionServices = roleModulePermissionServices;
            _userRoleServices = userRoleServices;
            _httpContext = httpContext;
            _user = user;
            _requirement = requirement;
            _redisCacheManager = redisCacheManager;
        }


        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <param name="page"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        // GET: api/User
        [HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]

        public async Task<MessageModel<PageModel<Permission>>> Get(int page = 1, string key = "")
        {
            PageModel<Permission> permissions = new PageModel<Permission>();
            int intPageSize = 50;
            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
            {
                key = "";
            }

            #region 舍弃
            //var permissions = await _permissionServices.Query(a => a.IsDeleted != true);
            //if (!string.IsNullOrEmpty(key))
            //{
            //    permissions = permissions.Where(t => (t.Name != null && t.Name.Contains(key))).ToList();
            //}
            ////筛选后的数据总数
            //totalCount = permissions.Count;
            ////筛选后的总页数
            //pageCount = (Math.Ceiling(totalCount.ObjToDecimal() / intTotalCount.ObjToDecimal())).ObjToInt();
            //permissions = permissions.OrderByDescending(d => d.Id).Skip((page - 1) * intTotalCount).Take(intTotalCount).ToList(); 
            #endregion



            permissions = await _permissionServices.QueryPage(a => a.IsDeleted != true && (a.Name != null && a.Name.Contains(key)), page, intPageSize, " Id desc ");


            #region 单独处理

            var apis = await _moduleServices.Query(d => d.IsDeleted == false);
            var permissionsView = permissions.data;

            var permissionAll = await _permissionServices.Query(d => d.IsDeleted != true);
            foreach (var item in permissionsView)
            {
                List<int> pidarr = new List<int>
                {
                    item.Pid
                };
                if (item.Pid > 0)
                {
                    pidarr.Add(0);
                }
                var parent = permissionAll.FirstOrDefault(d => d.Id == item.Pid);

                while (parent != null)
                {
                    pidarr.Add(parent.Id);
                    parent = permissionAll.FirstOrDefault(d => d.Id == parent.Pid);
                }


                item.PidArr = pidarr.OrderBy(d => d).Distinct().ToList();
                foreach (var pid in item.PidArr)
                {
                    var per = permissionAll.FirstOrDefault(d => d.Id == pid);
                    item.PnameArr.Add((per != null ? per.Name : "根节点") + "/");
                    //var par = Permissions.Where(d => d.Pid == item.Id ).ToList();
                    //item.PCodeArr.Add((per != null ? $"/{per.Code}/{item.Code}" : ""));
                    //if (par.Count == 0 && item.Pid == 0)
                    //{
                    //    item.PCodeArr.Add($"/{item.Code}");
                    //}
                }

                item.MName = apis.FirstOrDefault(d => d.Id == item.Mid)?.LinkUrl;
            }

            permissions.data = permissionsView;

            #endregion


            return new MessageModel<PageModel<Permission>>()
            {
                msg = "获取成功",
                success = permissions.dataCount >= 0,
                response = permissions
            };

        }

        /// <summary>
        /// 查询树形 Table
        /// </summary>
        /// <param name="f">父节点</param>
        /// <param name="key">关键字</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<MessageModel<List<Permission>>> GetTreeTable(int f = 0, string key = "")
        {
            List<Permission> permissions = new List<Permission>();
            var apiList = await _moduleServices.Query(d => d.IsDeleted == false);
            var permissionsList = await _permissionServices.Query(d => d.IsDeleted == false);
            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
            {
                key = "";
            }

            if (key != "")
            {
                permissions = permissionsList.Where(a => a.Name.Contains(key)).OrderBy(a => a.OrderSort).ToList();
            }
            else
            {
                permissions = permissionsList.Where(a => a.Pid == f).OrderBy(a => a.OrderSort).ToList();
            }

            foreach (var item in permissions)
            {
                List<int> pidarr = new List<int> { };
                var parent = permissionsList.FirstOrDefault(d => d.Id == item.Pid);

                while (parent != null)
                {
                    pidarr.Add(parent.Id);
                    parent = permissionsList.FirstOrDefault(d => d.Id == parent.Pid);
                }

                //item.PidArr = pidarr.OrderBy(d => d).Distinct().ToList();

                pidarr.Reverse();
                pidarr.Insert(0, 0);
                item.PidArr = pidarr;

                item.MName = apiList.FirstOrDefault(d => d.Id == item.Mid)?.LinkUrl;
                item.HasChildren = permissionsList.Where(d => d.Pid == item.Id).Any();
            }


            return new MessageModel<List<Permission>>()
            {
                msg = "获取成功",
                success = permissions.Count >= 0,
                response = permissions
            };
        }


        /// <summary>
        /// 新增/更新菜单
        /// </summary>
        /// <param name="permission"></param>
        /// <returns></returns>
        // POST: api/User
        [HttpPost]

        //[ApiExplorerSettings(IgnoreApi = true)]
        public async Task<MessageModel<string>> Post([FromBody] Permission permission)
        {
            var data = new MessageModel<string>();

            permission.Enabled = true;
            permission.IsDeleted = false;

            //根据mid，取出mname
            var module =await _moduleServices.QueryById(permission.Mid);
            if (module!=null) {
                permission.MName = module.LinkUrl;
            }
      

            if (permission != null && permission.Id > 0)
            {
                data.success = await _permissionServices.Update(permission);
                if (data.success)
                {
                    data.msg = "更新成功";
                    data.response = permission?.Id.ObjToString();
                    _redisCacheManager.Remove("GetNavigationBar");//清除查询列表缓存
                }
            }
            else
            {
                permission.CreateId = _user.ID;
                permission.CreateBy = _user.Name;

                var id = (await _permissionServices.Add(permission));
                data.success = id > 0;
                if (data.success)
                {
                    data.response = id.ObjToString();
                    data.msg = "添加成功";

                    _redisCacheManager.Remove("GetNavigationBar");//清除查询列表缓存
                }
            }
            return data;
        }

        /// <summary>
        /// 保存(菜单路由)权限--一次性保存，不是新增
        /// </summary>
        /// <param name="assignView"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<MessageModel<string>> Assign([FromBody] AssignPermissionView assignView)
        {
            var data = new MessageModel<string>();
            try
            {
                if (assignView.rid > 0)
                {
                    data.success = true;
                    var roleModulePermissions = await _roleModulePermissionServices.Query(d => d.RoleId == assignView.rid);
                    //var remove = roleModulePermissions.Select(c => (object)c.Id)ToArray();

                    //删除以前勾选，但是现在未勾选的
                    var remove = roleModulePermissions.Where(d => !assignView.pids.Contains(d.PermissionId.ObjToInt())).Select(c => (object)c.Id);
                    data.success &= remove.Any() ? await _roleModulePermissionServices.DeleteByIds(remove.ToArray()) : true;

                    var permissionlist =await _permissionServices.Query();
                    List<RoleModulePermission> rolemodulepermissionlist = new List<RoleModulePermission>();
                    foreach (var item in assignView.pids)
                    {
                        bool exists = (roleModulePermissions.Select(c => (object)c.PermissionId).ToArray()).Contains(item);
                        if (!exists) {
                            var moduleid = permissionlist.Where(a => a.Id == item).FirstOrDefault()?.Mid;
                            RoleModulePermission roleModulePermission = new RoleModulePermission()
                            {
                                IsDeleted = false,
                                RoleId = assignView.rid,
                                ModuleId = moduleid.ObjToInt(),
                                PermissionId = item,
                            };
                            roleModulePermission.CreateId = _user.ID;
                            roleModulePermission.CreateBy = _user.Name;

                            rolemodulepermissionlist.Add(roleModulePermission);
                            //修改为 批量插入
                            //await _roleModulePermissionServices.Add(roleModulePermission);
                        }
                    }
                    if (rolemodulepermissionlist.Count>0) {
                        await _roleModulePermissionServices.Add(rolemodulepermissionlist);
                    }
                    data.success = true;
                    if (data.success)
                    {
                        _requirement.Permissions.Clear();
                        data.response = "";
                        data.msg = "保存成功";
                    }

                }
            }
            catch (Exception ex)
            {
                data.success = false;
            }

            return data;
        }

        /// <summary>
        /// 菜单分配保存
        /// </summary>
        /// <param name="assignView"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<MessageModel<string>> AssignTenan([FromBody] AssignTenanView assignView)
        {
            var data = new MessageModel<string>();
            try
            {
                if (assignView.tid > 0)
                {
                    data.success = true;

                    var permissionTenan = await _permissionTenanServices.Query(d => d.TenanId == assignView.tid);
                    var permissionlist = await _permissionServices.Query();

                    //删除以前勾选，但是现在未勾选的
                    var remove = permissionTenan.Where(d => !assignView.pids.Contains(d.PermissionId.ObjToInt())).Select(c => (object)c.Id);
                    data.success &= remove.Any() ? await _permissionTenanServices.DeleteByIds(remove.ToArray()) : true;

                    List<PermissionTenan> rolemodulepermissionlist = new List<PermissionTenan>();
                    foreach (var item in assignView.pids)
                    {
                        bool exists = (permissionTenan.Select(c => (object)c.PermissionId).ToArray()).Contains(item);
                        if (!exists)
                        {
                            //var moduleid = (await _permissionServices.Query(p => p.Id == item)).FirstOrDefault();
                            var module = permissionlist.Where(a => a.Id == item).FirstOrDefault();

                            PermissionTenan permissiontenan = new PermissionTenan()
                            {
                                TenanId = assignView.tid,
                                PermissionId = module.Id,
                            };
                            permissiontenan.CreateId = _user.ID;
                            permissiontenan.CreateBy = _user.Name;
                            permissiontenan.IsDeleted = false;
                            rolemodulepermissionlist.Add(permissiontenan);
                            data.success = true;
                        }
                      
                    }
                    if (rolemodulepermissionlist.Count>0) {
                        await _permissionTenanServices.Add(rolemodulepermissionlist);
                    }

                    if (data.success)
                    {
                        _requirement.Permissions.Clear();
                        data.response = "";
                        data.msg = "保存成功";
                    }

                }
            }
            catch (Exception)
            {
                data.success = false;
            }

            return data;
        }


        /// <summary>
        /// 新增(菜单路由)权限
        /// </summary>
        /// <param name="assignView"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<MessageModel<string>> AddPermission([FromBody] AssignPermissionView assignView)
        {
            var data = new MessageModel<string>();

            try
            {
                if (assignView.rid > 0)
                {
                    data.success = true;

                    var roleModulePermissions = await _roleModulePermissionServices.Query(d => d.RoleId == assignView.rid);

                    var remove = roleModulePermissions.Where(d => !assignView.pids.Contains(d.PermissionId.ObjToInt())).Select(c => (object)c.Id);
                    //data.success &= remove.Any() ? await _roleModulePermissionServices.DeleteByIds(remove.ToArray()) : true;

                    foreach (var item in assignView.pids)
                    {
                        var rmpitem = roleModulePermissions.Where(d => d.PermissionId == item);
                        if (!rmpitem.Any())
                        {
                            var moduleid = (await _permissionServices.Query(p => p.Id == item)).FirstOrDefault()?.Mid;
                            RoleModulePermission roleModulePermission = new RoleModulePermission()
                            {
                                IsDeleted = false,
                                RoleId = assignView.rid,
                                ModuleId = moduleid.ObjToInt(),
                                PermissionId = item,
                            };


                            roleModulePermission.CreateId = _user.ID;
                            roleModulePermission.CreateBy = _user.Name;

                            data.success &= (await _roleModulePermissionServices.Add(roleModulePermission)) > 0;

                        }
                    }

                    if (data.success)
                    {
                        _requirement.Permissions.Clear();
                        data.response = "";
                        data.msg = "保存成功";
                    }

                }
            }
            catch (Exception)
            {
                data.success = false;
            }

            return data;
        }



        /// <summary>
        /// 保存(API接口)权限--一次性保存，不是新增
        /// </summary>
        /// <param name="assignView"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<MessageModel<string>> AssignModule([FromBody] AssignModuleView assignView)
        {
            var data = new MessageModel<string>();

            try
            {
                if (assignView.rid > 0)
                {
                    data.success = true;

                    var roleModulePermissions = await _roleModulePermissionServices.Query(d => d.RoleId == assignView.rid);

                    var remove = roleModulePermissions.Where(d => !assignView.mids.Contains(d.PermissionId.ObjToInt())).Select(c => (object)c.Id);
                    data.success &= remove.Any() ? await _roleModulePermissionServices.DeleteByIds(remove.ToArray()) : true;

                    foreach (var item in assignView.mids)
                    {
                        var rmpitem = roleModulePermissions.Where(d => d.ModuleId == item);
                        if (!rmpitem.Any())
                        {

                            RoleModulePermission roleModulePermission = new RoleModulePermission()
                            {
                                IsDeleted = false,
                                RoleId = assignView.rid,
                                ModuleId = item,
                                // PermissionId = item,
                            };


                            roleModulePermission.CreateId = _user.ID;
                            roleModulePermission.CreateBy = _user.Name;

                            data.success &= (await _roleModulePermissionServices.Add(roleModulePermission)) > 0;

                        }
                    }

                    if (data.success)
                    {
                        _requirement.Permissions.Clear();
                        data.response = "";
                        data.msg = "保存成功";
                    }

                }
            }
            catch (Exception)
            {
                data.success = false;
            }

            return data;
        }


        /// <summary>
        /// 新增(API接口)权限
        /// </summary>
        /// <param name="assignView"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<MessageModel<string>> AddModule([FromBody] AssignModuleView assignView)
        {
            var data = new MessageModel<string>();

            try
            {
                if (assignView.rid > 0)
                {
                    data.success = true;

                    var roleModulePermissions = await _roleModulePermissionServices.Query(d => d.RoleId == assignView.rid);

                    var remove = roleModulePermissions.Where(d => !assignView.mids.Contains(d.PermissionId.ObjToInt())).Select(c => (object)c.Id);
                    // data.success &= remove.Any() ? await _roleModulePermissionServices.DeleteByIds(remove.ToArray()) : true;

                    foreach (var item in assignView.mids)
                    {
                        var rmpitem = roleModulePermissions.Where(d => d.ModuleId == item);
                        if (!rmpitem.Any())
                        {

                            RoleModulePermission roleModulePermission = new RoleModulePermission()
                            {
                                IsDeleted = false,
                                RoleId = assignView.rid,
                                ModuleId = item,
                                // PermissionId = item,
                            };


                            roleModulePermission.CreateId = _user.ID;
                            roleModulePermission.CreateBy = _user.Name;

                            data.success &= (await _roleModulePermissionServices.Add(roleModulePermission)) > 0;

                        }
                    }

                    if (data.success)
                    {
                        _requirement.Permissions.Clear();
                        data.response = "";
                        data.msg = "保存成功";
                    }

                }
            }
            catch (Exception)
            {
                data.success = false;
            }

            return data;
        }


        /// <summary>
        /// 获取菜单树列表（根据当前客户）
        /// </summary>
        /// <param name="id">客户id</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<MessageModel<List<PermissionTree>>> GetPermissionTree(int id = 0)
        {
            var data = new MessageModel<List<PermissionTree>>();

            var pidlist = await _permissionTenanServices.Query(a => a.TenanId == id);
            List<PermissionTree> permissionTrees = new List<PermissionTree>() { };

            if (id==0) {
                var permissions = await _permissionServices.Query(a => a.Enabled == true && a.IsDeleted == false);

                permissionTrees = (from child in permissions
                                   where child.IsDeleted == false
                                   orderby child.Id
                                   select new PermissionTree
                                   {
                                       Value = child.Id,
                                       Label = child.Name,
                                       Pid = child.Pid,
                                       Isbtn = child.IsButton,
                                       Order = child.OrderSort,
                                   }).ToList();
            }

            if (pidlist.Count <= 0)
            {

            }
            else
            {
              var permissionmodel=  await _permissionServices.Query();
                foreach (var item in pidlist)
                {
                    var permissions = permissionmodel.Where(a => a.Id == item.PermissionId).SingleOrDefault();

                    permissionTrees.Add(new PermissionTree
                    {
                        Value = permissions.Id,
                        Label = permissions.Name,
                        Pid = permissions.Pid,
                        Isbtn = permissions.IsButton,
                        Order = permissions.OrderSort,
                    });
                }
            }
            PermissionTree rootRoot = new PermissionTree
            {

            };

            permissionTrees = permissionTrees.OrderBy(d => d.Order).ToList();

            RecursionHelper.LoopToAppendChildren(permissionTrees, rootRoot, id);

            data.success = true;
            if (data.success)
            {
                data.response = rootRoot.Children;
                data.msg = "获取成功";
            }

            return data;
        }


        /// <summary>
        /// 菜单分配，菜单树列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<MessageModel<List<PermissionTree>>> GetPermissionTreeAll()
        {
            var data = new MessageModel<List<PermissionTree>>();

            List<PermissionTree> permissionTrees = new List<PermissionTree>() { };

            var permissions = await _permissionServices.Query(a => a.Enabled == true && a.IsDeleted == false);

            permissionTrees = (from child in permissions
                               where child.IsDeleted == false
                               orderby child.Id
                               select new PermissionTree
                               {
                                   Value = child.Id,
                                   Label = child.Name,
                                   Pid = child.Pid,
                                   Isbtn = child.IsButton,
                                   Order = child.OrderSort,
                               }).ToList();

            PermissionTree rootRoot = new PermissionTree
            {

            };

            permissionTrees = permissionTrees.OrderBy(d => d.Order).ToList();


            RecursionHelper.LoopToAppendChildren(permissionTrees, rootRoot, 0);

            data.success = true;
            if (data.success)
            {
                data.response = rootRoot.Children;
                data.msg = "获取成功";
            }

            return data;
        }


        /// <summary>
        /// 获取路由树（根据登录用户，获取权限下菜单列表）
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        /*[ApiExplorerSettings(IgnoreApi = true)]*/
        //[CustomRoute(ApiVersions.V2, "GetNavigationBar")]
        public async Task<MessageModel<List<NavigationBar>>> GetNavigationBar(int uid)
        {
            //查找
            var tid = _user.TID;//客户ID

            #region ES搜索，搜不到再去查找
            Permission permission = new Permission()
            {
                Id = 7,
                Name = "xxxxx",
            };

            await _iESSever.ElasticLinqClient.IndexAsync(permission, t => t.Index("persons").Id(permission.Id));
            var list = await _iESSever.ElasticLinqClient.SearchAsync<Permission>(
                             p => p.Index("persons")
                                   //.Type("Persons")
                                   .Query(op => op.Match(//
                                          ss => ss.Field(//字段
                                                qq => qq.Id == permission.Id))));

            #endregion


            var data = new MessageModel<List<NavigationBar>>();
                var roleIds = new List<int>();
                //获取所有角色id
                roleIds = (await _userRoleServices.Query(d => d.IsDeleted == false && d.UserId == uid)).Select(d => d.RoleId.ObjToInt()).Distinct().ToList();
                var modulelist = await _moduleServices.Query();
                if (uid > 0)
                {
                    if (roleIds.Any())
                    {
                        var pids = (await _roleModulePermissionServices.Query(d => d.IsDeleted == false && roleIds.Contains(d.RoleId))).Select(d => d.PermissionId.ObjToInt()).Distinct();

                        var rolePermissionMoudles = (await _permissionServices.Query(d => pids.Contains(d.Id))).OrderBy(c => c.OrderSort);
                        foreach (var item in rolePermissionMoudles)
                        {
                            if (item.IsButton)
                            {
                                //var modulemodel = await _moduleServices.QueryById(item.Mid);
                                var modulemodel = modulelist.Where(a => a.Id == item.Mid).SingleOrDefault();
                                if (modulemodel != null)
                                {
                                    item.MName = modulemodel.LinkUrl;
                                    item.Mid = modulemodel.Id;
                                }
                            }
                        }
                        var permissionTrees = (from child in rolePermissionMoudles
                                               where child.IsDeleted == false
                                               orderby child.Id
                                               select new NavigationBar
                                               {
                                                   Id = child.Id,
                                                   Name = child.Name,
                                                   Pid = child.Pid,
                                                   Order = child.OrderSort,
                                                   Path = child.Code,
                                                   IconCls = child.Icon,
                                                   Func = child.Func,
                                                   IsHide = child.IsHide.ObjToBool(),
                                                   IsButton = child.IsButton.ObjToBool(),
                                                   ApiLink = child.MName,
                                                   Mid = child.Mid,
                                                   Enabled = child.Enabled,
                                                   Description = child.Description
                                               }).ToList();
                        NavigationBar rootRoot = new NavigationBar()
                        {
                        };
                        RecursionHelper.LoopNaviBarAppendChildren(permissionTrees, rootRoot);
                        ;
                        data.success = true;
                        if (data.success)
                        {
                            data.response = rootRoot.Children;
                            data.msg = "获取成功";
                        }
                    }
                }
                if (uid == 0 || uid == 1)
                {
                if (_redisCacheManager.Get<object>("GetNavigationBar") != null)
                {
                    return _redisCacheManager.Get<MessageModel<List<NavigationBar>>>("GetNavigationBar");
                }
                else
                {
                    Expression<Func<Permission, Modules, bool>> whereExpression = (rmp, p) => rmp.IsDeleted == false && rmp.Enabled == true;
                    var rolePermissionMoudles = (await _permissionServices.Query()).OrderBy(c => c.OrderSort);
                    foreach (var item in rolePermissionMoudles)
                    {
                        if (item.IsButton)
                        {
                            //var modulemodel = await _moduleServices.QueryById(item.Mid);
                            var modulemodel = modulelist.Where(a => a.Id == item.Mid).SingleOrDefault();
                            if (modulemodel != null)
                            {
                                item.MName = modulemodel.LinkUrl;
                                item.Mid = modulemodel.Id;
                            }
                        }

                    }
                    var permissionTrees = (from child in rolePermissionMoudles
                                           where child.IsDeleted == false
                                           orderby child.Id
                                           select new NavigationBar
                                           {
                                               Enabled = child.Enabled,
                                               Id = child.Id,
                                               Name = child.Name,
                                               Pid = child.Pid,
                                               Order = child.OrderSort,
                                               Path = child.Code,
                                               IconCls = child.Icon,
                                               Func = child.Func,
                                               IsHide = child.IsHide.ObjToBool(),
                                               IsButton = child.IsButton.ObjToBool(),
                                               ApiLink = child.MName,
                                               Mid = child.Mid
                                           }).ToList();
                    NavigationBar rootRoot = new NavigationBar()
                    {
                    };
                    RecursionHelper.LoopNaviBarAppendChildren(permissionTrees, rootRoot);
                    ;
                    data.success = true;
                    if (data.success)
                    {
                        data.response = rootRoot.Children;
                        data.msg = "获取成功";
                    }
                    _redisCacheManager.Set("GetNavigationBar", data, TimeSpan.FromHours(0.5));//缓存30分钟
                }


              
                }
    
            return data;
            

        
        }


        /// <summary>
        /// 根据客户id，查询客户能分配的菜单
        /// </summary>
        /// <param name="tid"></param>
        /// <returns></returns>
        [HttpGet]
        /*[ApiExplorerSettings(IgnoreApi = true)]*/
        public async Task<MessageModel<List<NavigationBar>>> GetNavigationBarByTid(int tid)
        {

            var data = new MessageModel<List<NavigationBar>>();
            var roleIds = new List<int>();

            //先根据登录用户，获取所有角色信息
            roleIds = (await _tenanServices.Query(d => d.Enabled == true && d.Id == tid)).Select(d => d.Id.ObjToInt()).Distinct().ToList();


            if (roleIds.Any())
            {
                var pids = (await _roleModulePermissionServices.Query(d => d.IsDeleted == false && roleIds.Contains(d.RoleId))).Select(d => d.PermissionId.ObjToInt()).Distinct();
                if (pids.Any())
                {
                    var rolePermissionMoudles = (await _permissionServices.Query(d => pids.Contains(d.Id))).OrderBy(c => c.OrderSort);
                    foreach (var item in rolePermissionMoudles)
                    {
                        var modulemodel = await _moduleServices.QueryById(item.Mid);
                        if (modulemodel != null)
                        {
                            item.MName = modulemodel.LinkUrl;
                        }
                    }
                    var permissionTrees = (from child in rolePermissionMoudles
                                           where child.IsDeleted == false
                                           orderby child.Id
                                           select new NavigationBar
                                           {
                                               Id = child.Id,
                                               Name = child.Name,
                                               Pid = child.Pid,
                                               Order = child.OrderSort,
                                               Path = child.Code,
                                               IconCls = child.Icon,
                                               Func = child.Func,
                                               IsHide = child.IsHide.ObjToBool(),
                                               IsButton = child.IsButton.ObjToBool(),
                                               ApiLink = child.MName
                                           }).ToList();
                    NavigationBar rootRoot = new NavigationBar()
                    {
                    };
                    permissionTrees = permissionTrees.OrderBy(d => d.Order).ToList();
                    RecursionHelper.LoopNaviBarAppendChildren(permissionTrees, rootRoot);
                    ;
                    data.success = true;
                    if (data.success)
                    {
                        data.response = rootRoot.Children;
                        data.msg = "获取成功";
                    }
                }
            }

            return data;
        }

        /// <summary>
        /// 获取API树（根据登录用户，获取权限下API接口）
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <returns></returns>
        [HttpGet]
        /*[ApiExplorerSettings(IgnoreApi = true)]*/
        public async Task<MessageModel<List<NavigationBar>>> GetModuleBar(int uid)
        {

            var data = new MessageModel<List<NavigationBar>>();

            var uidInHttpcontext1 = 0;
            var roleIds = new List<int>();
            // ids4和jwt切换
            if (Permissions.IsUseIds4)
            {
                // ids4
                uidInHttpcontext1 = (from item in _httpContext.HttpContext.User.Claims
                                     where item.Type == "sub"
                                     select item.Value).FirstOrDefault().ObjToInt();
                roleIds = (from item in _httpContext.HttpContext.User.Claims
                           where item.Type == "role"
                           select item.Value.ObjToInt()).ToList();
            }
            else
            {
                // jwt
                uidInHttpcontext1 = ((JwtHelper.SerializeJwt(_httpContext.HttpContext.Request.Headers["Authorization"].ObjToString().Replace("Bearer ", "")))?.Uid).ObjToInt();
                roleIds = (await _userRoleServices.Query(d => d.IsDeleted == false && d.UserId == uid)).Select(d => d.RoleId.ObjToInt()).Distinct().ToList();
            }


            if (uid > 0 && uid == uidInHttpcontext1)
            {
                if (roleIds.Any())
                {
                    var pids = (await _roleModulePermissionServices.Query(d => d.IsDeleted == false && roleIds.Contains(d.RoleId))).Select(d => d.ModuleId.ObjToInt()).Distinct();
                    if (pids.Any())
                    {
                        var rolePermissionMoudles = (await _moduleServices.Query(d => pids.Contains(d.Id))).OrderBy(c => c.OrderSort);
                        var permissionTrees = (from child in rolePermissionMoudles
                                               where child.IsDeleted == false
                                               orderby child.Id
                                               select new NavigationBar
                                               {
                                                   Id = child.Id,
                                                   Name = child.Name,
                                                   Pid = 0,
                                                   Order = child.OrderSort,
                                                   Path = child.LinkUrl,
                                                   IconCls = child.Icon,
                                               }).ToList();

                        permissionTrees = permissionTrees.OrderBy(d => d.Order).ToList();
                        data.success = true;
                        if (data.success)
                        {
                            data.response = permissionTrees;
                            data.msg = "获取成功";
                        }
                    }
                }
            }
            return data;
        }



        /// <summary>
        /// 获取路由列表（根据登录用户，获取权限下菜单路由）
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        [HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<MessageModel<NavigationBar>> GetNavigationBar1(int uid)
        {

            var data = new MessageModel<NavigationBar>();

            var uidInHttpcontext1 = 0;
            var roleIds = new List<int>();
            // ids4和jwt切换
            if (Permissions.IsUseIds4)
            {
                // ids4
                uidInHttpcontext1 = (from item in _httpContext.HttpContext.User.Claims
                                     where item.Type == "sub"
                                     select item.Value).FirstOrDefault().ObjToInt();
                roleIds = (from item in _httpContext.HttpContext.User.Claims
                           where item.Type == "role"
                           select item.Value.ObjToInt()).ToList();
            }
            else
            {
                // jwt
                uidInHttpcontext1 = ((JwtHelper.SerializeJwt(_httpContext.HttpContext.Request.Headers["Authorization"].ObjToString().Replace("Bearer ", "")))?.Uid).ObjToInt();
                roleIds = (await _userRoleServices.Query(d => d.IsDeleted == false && d.UserId == uid)).Select(d => d.RoleId.ObjToInt()).Distinct().ToList();
            }


            if (uid > 0 && uid == uidInHttpcontext1)
            {
                if (roleIds.Any())
                {
                    var pids = (await _roleModulePermissionServices.Query(d => d.IsDeleted == false && roleIds.Contains(d.RoleId))).Select(d => d.PermissionId.ObjToInt()).Distinct();
                    if (pids.Any())
                    {
                        var rolePermissionMoudles = (await _permissionServices.Query(d => pids.Contains(d.Id))).OrderBy(c => c.OrderSort);
                        var permissionTrees = (from child in rolePermissionMoudles
                                               where child.IsDeleted == false
                                               orderby child.Id
                                               select new NavigationBar
                                               {
                                                   Id = child.Id,
                                                   Name = child.Name,
                                                   Pid = child.Pid,
                                                   Order = child.OrderSort,
                                                   Path = child.Code,
                                                   IconCls = child.Icon,
                                                   Func = child.Func,
                                                   IsHide = child.IsHide.ObjToBool(),
                                                   IsButton = child.IsButton.ObjToBool(),
                                                   Meta = new NavigationBarMeta
                                                   {
                                                       RequireAuth = true,
                                                       Title = child.Name,
                                                       NoTabPage = child.IsHide.ObjToBool(),
                                                       KeepAlive = child.IskeepAlive.ObjToBool()
                                                   }
                                               }).ToList();


                        NavigationBar rootRoot = new NavigationBar()
                        {
                        };

                        permissionTrees = permissionTrees.OrderBy(d => d.Order).ToList();

                        RecursionHelper.LoopNaviBarAppendChildren(permissionTrees, rootRoot);

                        ;

                        data.success = true;
                        if (data.success)
                        {
                            data.response = rootRoot;
                            data.msg = "获取成功";
                        }
                    }
                }
            }
            return data;
        }


        /// <summary>
        /// 通过角色获取菜单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<MessageModel<AssignShow>> GetPermissionIdByRoleId(int id = 0)
        {
            var data = new MessageModel<AssignShow>();

            var rmps = await _roleModulePermissionServices.Query(d => d.IsDeleted == false && d.RoleId == id);
            var permissionTrees = (from child in rmps
                                   orderby child.Id
                                   select child.PermissionId.ObjToInt()).ToList();

            var permissions = await _permissionServices.Query(d => d.IsDeleted == false);
            List<string> assignbtns = new List<string>();

            data.success = true;
            if (data.success)
            {
                data.response = new AssignShow()
                {
                    permissionids = permissionTrees,
                    //assignbtns = assignbtns,
                };
                data.msg = "获取成功";
            }

            return data;
        }

        /// <summary>
        /// 通过客户id获取已分配的菜单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<MessageModel<AssignShow>> GetPermissionIdByTenanId(int id = 0)
        {
            var data = new MessageModel<AssignShow>();

            var rmps = await _permissionTenanServices.Query(d => d.IsDeleted == false && d.TenanId == id);
            var permissionTrees = (from child in rmps
                                   select child.PermissionId.ObjToInt()).ToList();

            var permissions = await _permissionServices.Query(d => d.IsDeleted == false);
            List<string> assignbtns = new List<string>();

            //foreach (var item in permissionTrees)
            //{
            //    var pername = permissions.FirstOrDefault(d => d.IsButton && d.Id == item)?.Name;
            //    if (!string.IsNullOrEmpty(pername))
            //    {
            //        assignbtns.Add(pername + "_" + item);
            //        permissionTrees.Add();
            //    }
            //}

            data.success = true;
            if (data.success)
            {
                data.response = new AssignShow()
                {
                    permissionids = permissionTrees,
                    //assignbtns = assignbtns,
                };
                data.msg = "获取成功";
            }

            return data;
        }



        /// <summary>
        /// 更新菜单
        /// </summary>
        /// <param name="permission"></param>
        /// <returns></returns>
        // PUT: api/User/5
        [HttpPut]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<MessageModel<string>> Put([FromBody] Permission permission)
        {
            var data = new MessageModel<string>();
            if (permission != null && permission.Id > 0)
            {
                data.success = await _permissionServices.Update(permission);
                if (data.success)
                {
                    data.msg = "更新成功";
                    data.response = permission?.Id.ObjToString();
                }
            }

            return data;
        }

        /// <summary>
        /// 禁用/启用菜单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE: api/ApiWithActions/5
        [HttpGet]
        public async Task<MessageModel<string>> DeleteOrActivation(int id)
        {
            var data = new MessageModel<string>();
            if (id > 0)
            {
                var userDetail = await _permissionServices.QueryById(id);
                userDetail.Enabled = !userDetail.Enabled;
                data.success = await _permissionServices.Update(userDetail);
                if (data.success)
                {
                    data.msg = "操作成功";
                    data.response = userDetail?.Id.ObjToString();
                    _redisCacheManager.Remove("GetNavigationBar");//清除查询列表缓存
                }
            }
            return data;
        }

        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<MessageModel<string>> Delete(int id)
        {
            var data = new MessageModel<string>();
            if (id > 0)
            {
                var userDetail = await _permissionServices.QueryById(id);
                userDetail.IsDeleted = true;
                data.success = await _permissionServices.Update(userDetail);
                if (data.success)
                {
                    data.msg = "删除成功";
                    data.response = userDetail?.Id.ObjToString();
                    _redisCacheManager.Remove("GetNavigationBar");//清除查询列表缓存
                }
            }
            return data;
        }

    }

    public class AssignPermissionView
    {
        /// <summary>
        /// 菜单ID集合
        /// </summary>
        public List<int> pids { get; set; }
        /// <summary>
        /// 角色ID
        /// </summary>
        public int rid { get; set; }
    }
    public class AssignAchPermissionView
    {
        /// <summary>
        /// 菜单ID集合
        /// </summary>
        public List<string> pids { get; set; }
        /// <summary>
        /// 角色ID
        /// </summary>
        public string rolid { get; set; }
        /// <summary>
        /// 客户id
        /// </summary>
        public int tid { get; set; }
    }


    public class AssignTenanView
    {
        /// <summary>
        /// 菜单ID集合
        /// </summary>
        public List<int> pids { get; set; }
        /// <summary>
        /// 客户ID
        /// </summary>
        public int tid { get; set; }
    }

    public class AssignModuleView
    {
        /// <summary>
        /// API接口ID集合
        /// </summary>
        public List<int> mids { get; set; }
        /// <summary>
        /// 角色ID
        /// </summary>
        public int rid { get; set; }
    }
    public class AssignShow
    {
        public List<int> permissionids { get; set; }
        //public List<string> assignbtns { get; set; }
    }
    public class AssignStringShow
    {
        public List<string> permissionids { get; set; }
        //public List<string> assignbtns { get; set; }
    }


}
