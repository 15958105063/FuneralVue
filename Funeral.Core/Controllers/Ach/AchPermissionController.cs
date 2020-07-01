
using AutoMapper;
using Funeral.Core.Common.Helper;
using Funeral.Core.Common.HttpContextUser;
using Funeral.Core.IServices;
using Funeral.Core.Model;
using Funeral.Core.Model.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Funeral.Core.Controllers
{
    /// <summary>
    /// 初始化 菜单按钮和字段管理
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]

    public class AchPermissionController : ControllerBase
    {
        private readonly IAchRacServices _achRacServices;
        private readonly INpoiWordExportServices _npoiWordExportServices;
        private readonly IMapper _mapper;
        readonly IUser _user;
        public AchPermissionController(IAchRacServices achRacServices, INpoiWordExportServices npoiWordExportServices, IUser user, IMapper mapper)
        {
            this._achRacServices = achRacServices;
            this._npoiWordExportServices = npoiWordExportServices;
            this._mapper = mapper;
            this._user = user;
        }


        /// <summary>
        /// 通过所选角色，获取所配置的按钮
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpGet]
        [AllowAnonymous]
        public async Task<MessageModel<AssignStringShow>> GetPermissionIdByRoleId(string id = "")
        {
            var data = new MessageModel<AssignStringShow>();
            //先根据角色按钮关联表 取出所有设置的按钮id
            var ids = await _achRacServices.Query(a => a.RacRolid == id);
            var permissionTrees = (from child in ids
                                   select child.RacBtnid).ToList();
            data.success = true;
            if (data.success)
            {
                data.response = new AssignStringShow()
                {
                    permissionids = permissionTrees,
                    //assignbtns = assignbtns,
                };
                data.msg = "获取成功";
            }
            return data;
        }

        ///// <summary>
        ///// 根据客户id，获取菜单列表
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //[HttpGet]
        //public async Task<MessageModel<List<PermissionTree>>> GetPermissionTree(int id = 0)
        //{
        //    var data = new MessageModel<List<PermissionTree>>();

        //    var list=await 

        //    var pidlist = await _permissionTenanServices.Query(a => a.TenanId == id);
        //    List<PermissionTree> permissionTrees = new List<PermissionTree>() { };
        //        foreach (var item in pidlist)
        //        {

        //            var permissions = await _permissionServices.QueryById(item.PermissionId);

        //            permissionTrees.Add(new PermissionTree
        //            {
        //                Value = permissions.Id,
        //                Label = permissions.Name,
        //                Pid = permissions.Pid,
        //                Isbtn = permissions.IsButton,
        //                Order = permissions.OrderSort,
        //            });
        //        }

        //    PermissionTree rootRoot = new PermissionTree
        //    {

        //    };

        //    permissionTrees = permissionTrees.OrderBy(d => d.Order).ToList();


        //    RecursionHelper.LoopToAppendChildren(permissionTrees, rootRoot, id);

        //    data.success = true;
        //    if (data.success)
        //    {
        //        data.response = rootRoot.Children;
        //        data.msg = "获取成功";
        //    }

        //    return data;
        //}



    }
}
