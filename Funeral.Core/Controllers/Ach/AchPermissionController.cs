
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
        /// 根据客户id查询菜单配置信息（四级）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //[HttpGet]
        //[AllowAnonymous]
        //public async Task<MessageModel<List<NavigationBar>>> GetNavigationBar(int id)
        //{
        //    //由于四级的表都不一样，所以这里需要手动添加四级

        //    var data = new MessageModel<List<NavigationBar>>();
        //    var roleIds = new List<int>();

        //        var rolePermissionMoudles = (await _permissionServices.Query()).OrderBy(c => c.OrderSort);
        //        foreach (var item in rolePermissionMoudles)
        //        {
        //            if (item.IsButton)
        //            {
        //                var modulemodel = await _moduleServices.QueryById(item.Mid);
        //                if (modulemodel != null)
        //                {
        //                    item.MName = modulemodel.LinkUrl;
        //                }
        //            }

        //        }
        //        var permissionTrees = (from child in rolePermissionMoudles
        //                               where child.IsDeleted == false
        //                               orderby child.Id
        //                               select new NavigationBar
        //                               {
        //                                   Id = child.Id,
        //                                   Name = child.Name,
        //                                   Pid = child.Pid,
        //                                   Order = child.OrderSort,
        //                                   Path = child.Code,
        //                                   IconCls = child.Icon,
        //                                   Func = child.Func,
        //                                   IsHide = child.IsHide.ObjToBool(),
        //                                   IsButton = child.IsButton.ObjToBool(),
        //                                   ApiLink = child.MName
        //                               }).ToList();
        //        NavigationBar rootRoot = new NavigationBar()
        //        {
        //        };
        //        RecursionHelper.LoopNaviBarAppendChildren(permissionTrees, rootRoot);
        //        ;
        //        data.success = true;
        //        if (data.success)
        //        {
        //            data.response = rootRoot.Children;
        //            data.msg = "获取成功";
        //        }
        //    return data;
        //}

    }
}
