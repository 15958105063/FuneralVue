
using AutoMapper;
using Funeral.Core.Common.Helper;
using Funeral.Core.Common.HttpContextUser;
using Funeral.Core.IServices;
using Funeral.Core.Model;
using Funeral.Core.Model.Models;
using Funeral.Core.Model.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IAchFgpServices _achFgpServices;
        private readonly IAchFupServices _achFupServices;
        private readonly IAchFunServices _achFunServices;
        private readonly IAchBtnServices _achBtnServices;

        private readonly IAchRacServices _achRacServices;
        private readonly INpoiWordExportServices _npoiWordExportServices;
        private readonly IMapper _mapper;
        readonly IUser _user;
        public AchPermissionController(IAchBtnServices achBtnServices, IAchFupServices achFupServices, IAchFunServices achFunServices, IAchFgpServices achFgpServices, IAchRacServices achRacServices, INpoiWordExportServices npoiWordExportServices, IUser user, IMapper mapper)
        {
            this._achBtnServices = achBtnServices;
            this._achFupServices = achFupServices;
            this._achFunServices = achFunServices;
            this._achFgpServices = achFgpServices;
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
        [HttpGet]
        [AllowAnonymous]
        public async Task<MessageModel<List<NavigationBarConfiguar>>> GetNavigationBarConfiguar(int id)
        {
            var data = new MessageModel<List<NavigationBarConfiguar>>();
            var roleIds = new List<int>();

          List<NavigationBarConfiguar> rootRootlist1 = new List<NavigationBarConfiguar>()
            {
            };
            List<NavigationBarConfiguar> rootRootlist2 = new List<NavigationBarConfiguar>()
            {
            };
            List<NavigationBarConfiguar> rootRootlist3 = new List<NavigationBarConfiguar>()
            {
            };
            List<NavigationBarConfiguar> rootRootlist4 = new List<NavigationBarConfiguar>()
            {
            };
            var permission1= await _achFgpServices.Query(a=>a.Tid== id);
            foreach (var item1 in permission1) {
                //获取第二级别菜单
                var permission2 = await _achFupServices.Query(a => a.Tid == id&&a.FupFgpid== item1.FgpId);
                foreach (var item2 in permission2) {

                    //获取第三级菜单
                    var permission3 = await _achFunServices.Query(a => a.Tid == id && a.FunFupid == item2.FupId);
                    foreach (var item3 in permission3)
                    {
                         var permission4 = await _achBtnServices.Query(a => a.Tid == id && a.BtnFunId == item3.FunId);

                        foreach (var item4 in permission4) {

                            rootRootlist3.Add(new NavigationBarConfiguar
                            {
                                Id = item4.BtnId,
                                Name = item4.BtnName,
                                Pid = item3.FunId,
                                Path = "",
                                IsButton = true,
                                ImagePath = "",
                                Children = null
                            });
                        }
                        rootRootlist3.Add(new NavigationBarConfiguar
                        {
                            Id = item3.FunId,
                            Name = item3.FunName,
                            Pid = item2.FupId,
                            Path = item3.FunUrl,
                            IsButton = false,
                            ImagePath = item3.FunImageurl,
                            Children = rootRootlist4
                        });
                    }


                        rootRootlist2.Add(new NavigationBarConfiguar
                    {
                        Id = item2.FupId,
                        Name = item2.FupName,
                        Pid = item1.FgpId,
                        Path = "",
                        IsButton = false,
                        ImagePath = item2.FupImageurl,
                        Children = rootRootlist3
                        });
                }
                rootRootlist1.Add(new NavigationBarConfiguar
                {
                    Id = item1.FgpId,
                    Name = item1.FgpName,
                    Pid = "",
                    Path = "",
                    IsButton = false,
                    ImagePath="",
                    Children= rootRootlist2
                });
            }

            data.success = true;
            if (data.success)
            {
                data.response = rootRootlist1;
                data.msg = "获取成功";
            }

            return data;
        }


        /// <summary>
        /// 新增/修改菜单配置信息
        /// </summary>
        /// <param name="permission"></param>
        /// <returns></returns>
        [HttpPost]

        //[ApiExplorerSettings(IgnoreApi = true)]
        public async Task<MessageModel<string>> Post([FromBody] AchPermissionInputViewModels permission)
        {
            //这里根据父级id，插入或者更新不同的表
            
            var data = new MessageModel<string>();

            var inserttype = "AchFgp";//第一级菜单

            #region 判断传入的是针对哪个表的操作，即根据父级id查找
            //这里可以考虑做枚举，目前先写死
            if (!string.IsNullOrEmpty(permission.Pid))
            {

                #region 第二级菜单
                var fgp = await _achFgpServices.Query(a=>a.FgpId== permission.Pid);
                if (fgp.Count>0) {
                    inserttype = "AchFup";
                }
                #endregion
                #region 第三级菜单
                var fup = await _achFupServices.Query(a => a.FupId == permission.Pid);
                if (fup.Count > 0)
                {
                    inserttype = "AchFun";
                }
                #endregion
                #region 第四级菜单
                var fun = await _achFunServices.Query(a => a.FunId == permission.Pid);
                if (fun.Count > 0)
                {
                    inserttype = "AchBtn";
                }
                #endregion
            
            }
            else {
                inserttype = "AchFgp";
            }

            #endregion

            switch (inserttype) {
                case "AchFgp":
                    //自动映射还是手动映射？
                    //先考虑手动，因为自动，可能需要建立多个映射关系，还需要指向字段
                    AchFgp achfgp = new AchFgp
                    {
                        Id = permission.Id,
                        FgpId= permission.AchId,
                        FgpName = permission.Name,
                        FgpOrgid = permission.OrgId,
                        FgpProname= permission.ProName,
                        FgpProtype= permission.FgpProtype,
                        FgpPruid= permission.FgpPruid,
                        FgpValue = permission.FgpValue,
                        FgpImageurl = permission.FgpImageurl,
                        FgpNum = permission.FgpNum,
                    };
                    if (achfgp != null && achfgp.Id > 0)
                    {
                        //更新
                        data.success = await _achFgpServices.Update(achfgp);
                        if (data.success)
                        {
                            data.msg = "更新成功";
                            data.response = achfgp?.Id.ObjToString();
                        }
                    }
                    else {
                        //新增
                        var id = (await _achFgpServices.Add(achfgp));
                        data.success = id > 0;
                        if (data.success)
                        {
                            data.response = id.ObjToString();
                            data.msg = "添加成功";
                        }
                    }
                        break;
                case "AchFup":
                    AchFup achfup = new AchFup
                    {
                        Id = permission.Id,
                        FupId = permission.AchId,
                        FupName = permission.Name,
                        FupFgpid = permission.Pid,
                        FupShowname = permission.ProName,
                        FupValue = permission.FgpValue,
                        FupImageurl = permission.FgpImageurl,
                        FupNum = permission.FgpNum,
                    };
                    if (achfup != null && achfup.Id > 0)
                    {
                        //更新
                        data.success = await _achFupServices.Update(achfup);
                        if (data.success)
                        {
                            data.msg = "更新成功";
                            data.response = achfup?.Id.ObjToString();
                        }
                    }
                    else
                    {
                        //新增
                        var id = (await _achFupServices.Add(achfup));
                        data.success = id > 0;
                        if (data.success)
                        {
                            data.response = id.ObjToString();
                            data.msg = "添加成功";
                        }
                    }
                    break;
                case "AchFun":
                    AchFun achfun = new AchFun
                    {
                        Id = permission.Id,
                        FunId = permission.AchId,
                        FunName = permission.Name,
                        FunFupid = permission.Pid,
                        FunShowname = permission.ProName,
                        FunValue = permission.FgpValue,
                        FunImageurl = permission.FgpImageurl,
                        FunNum = permission.FgpNum,
                        FunUrl= permission.Code,

                    };
                    if (achfun != null && achfun.Id > 0)
                    {
                        //更新
                        data.success = await _achFunServices.Update(achfun);
                        if (data.success)
                        {
                            data.msg = "更新成功";
                            data.response = achfun?.Id.ObjToString();
                        }
                    }
                    else
                    {
                        //新增
                        var id = (await _achFunServices.Add(achfun));
                        data.success = id > 0;
                        if (data.success)
                        {
                            data.response = id.ObjToString();
                            data.msg = "添加成功";
                        }
                    }
                    break;
                case "AchBtn":
                    AchBtn achbtn = new AchBtn
                    {
                        Id = permission.Id,
                        BtnId = permission.AchId,
                        BtnName = permission.Name,
                        BtnFunId = permission.Pid,
                        BtnShowName = permission.ProName,
                        BtnValue = permission.FgpValue,
                        BtnNum = permission.FgpNum,
                    };
                    if (achbtn != null && achbtn.Id > 0)
                    {
                        //更新
                        data.success = await _achBtnServices.Update(achbtn);
                        if (data.success)
                        {
                            data.msg = "更新成功";
                            data.response = achbtn?.Id.ObjToString();
                        }
                    }
                    else
                    {
                        //新增
                        var id = (await _achBtnServices.Add(achbtn));
                        data.success = id > 0;
                        if (data.success)
                        {
                            data.response = id.ObjToString();
                            data.msg = "添加成功";
                        }
                    }
                    break;

            }

            return data;
        }

    }
}
