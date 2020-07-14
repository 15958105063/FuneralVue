
using AutoMapper;
using Funeral.Core.Common.Helper;
using Funeral.Core.Common.HttpContextUser;
using Funeral.Core.IServices;
using Funeral.Core.Model;
using Funeral.Core.Model.Models;
using Funeral.Core.Model.ViewModels;
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
            var permission1 = await _achFgpServices.Query(a => a.Tid == id);
            foreach (var item1 in permission1)
            {
             
                List<NavigationBarConfiguar> rootRootlist2 = new List<NavigationBarConfiguar>()
                {
                };
                List<NavigationBarConfiguar> rootRootlist3 = new List<NavigationBarConfiguar>()
                {
                };
                List<NavigationBarConfiguar> rootRootlist4 = new List<NavigationBarConfiguar>()
                {
                };

                //获取第二级别菜单
                var permission2 = await _achFupServices.Query(a => a.Tid == id && a.FupFgpid == item1.FgpId);
                foreach (var item2 in permission2)
                {
                    //获取第三级菜单
                    var permission3 = await _achFunServices.Query(a => a.Tid == id && a.FunFupid == item2.FupId);
                    foreach (var item3 in permission3)
                    {
                        var permission4 = await _achBtnServices.Query(a => a.Tid == id && a.BtnFunId == item3.FunId);
                        foreach (var item4 in permission4)
                        {
                            rootRootlist4.Add(new NavigationBarConfiguar
                            {
                                AchId= item4.BtnId,
                                Id = item4.BtnId,
                                Name = item4.BtnValue,
                                Value = item4.BtnValue,
                                Pid = item3.FunId,
                                Path = "",
                                IsButton = true,
                                ImagePath = "",
                                Num = item4.BtnNum,
                        
                                Children = null
                            });
                        }
                        rootRootlist3.Add(new NavigationBarConfiguar
                        {
                            AchId = item3.FunId,
                            Id = item3.FunId,
                            Name = item3.FunValue,
                            Value = item3.FunValue,
                            Pid = item2.FupId,
                            Path = item3.FunUrl,
                            IsButton = false,
                            ImagePath = item3.FunImageurl,
                            Num = item3.FunNum,
              
                            Children = rootRootlist4.Count>0? rootRootlist4 : null
                        });
                    }


                    rootRootlist2.Add(new NavigationBarConfiguar
                    {
                        AchId = item2.FupId,
                        Id = item2.FupId,
                        Name = item2.FupValue,
                        Value = item2.FupValue,
                        Pid = item1.FgpId,
                        Path = "",
                        IsButton = false,
                        ImagePath = item2.FupImageurl,
                        Num = item2.FupNum,
                      
                        Children = rootRootlist3.Count>0? rootRootlist3 : null
                    });
                }
                rootRootlist1.Add(new NavigationBarConfiguar
                {
                    AchId = item1.FgpId,
                    Id = item1.FgpId,
                    Name = item1.FgpValue,
                    Value = item1.FgpValue,
                    Pid = "",
                    Path = "",
                    IsButton = false,

                    ImagePath = item1.FgpImageurl,
                    OrgId= item1.FgpOrgid,
                    ProName= item1.FgpProname,
                    ProType= item1.FgpProtype,
                    Pruid= item1.FgpPruid,
                    Num= item1.FgpNum,
                
                    Children = rootRootlist2.Count > 0 ? rootRootlist2 :null
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
                var fgp = await _achFgpServices.Query(a => a.FgpId == permission.Pid);
                if (fgp.Count > 0)
                {
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
            else
            {
                inserttype = "AchFgp";
            }

            #endregion

            switch (inserttype)
            {
                case "AchFgp":
                    //自动映射还是手动映射？
                    //先考虑手动，因为自动，可能需要建立多个映射关系，还需要指向字段
                    AchFgp achfgp = new AchFgp
                    {
                        Tid = permission.TId,
                        Id = permission.Id,
                        FgpId = permission.AchId,
                        FgpName = permission.Name,
                        FgpOrgid = permission.OrgId,
                        FgpProname = permission.ProName,
                        FgpProtype = permission.ProType,
                        FgpPruid = permission.Pruid,
                        FgpValue = permission.Value,
                        FgpImageurl = permission.Imageurl,
                        FgpNum = permission.Num,
               
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
                    else
                    {
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
                        Tid = permission.TId,
                        Id = permission.Id,
                        FupId = permission.AchId,
                        FupName = permission.Name,
                        FupFgpid = permission.Pid,
                        FupShowname = permission.ProName,
                        FupValue = permission.Value,
                        FupImageurl = permission.Imageurl,
                        FupNum = permission.Num,

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
                        Tid = permission.TId,
                        Id = permission.Id,
                        FunId = permission.AchId,
                        FunName = permission.Name,
                        FunFupid = permission.Pid,
                        FunShowname = permission.ProName,
                        FunValue = permission.Value,
                        FunImageurl = permission.Imageurl,
                        FunNum = permission.Num,
                        FunUrl = permission.Code,
     

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
                        Tid = permission.TId,
                        Id = permission.Id,
                        BtnId = permission.AchId,
                        BtnName = permission.Name,
                        BtnFunId = permission.Pid,
                        BtnShowName = permission.ProName,
                        BtnValue = permission.Value,
                        BtnNum = permission.Num,
       
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


        /// <summary>
        /// 根据菜单achid编码获取菜单信息
        /// </summary>
        /// <param name="achid"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<MessageModel<AchPermissionInputViewModels>> GetByAchId(string achid)
        {
            AchPermissionInputViewModels models = new AchPermissionInputViewModels() { };

            //这里可以考虑做枚举，目前先写死
            if (!string.IsNullOrEmpty(achid))
            {
                #region 第一级菜单
                var fgp = (await _achFgpServices.Query(a => a.FgpId == achid)).FirstOrDefault();
                if (fgp != null)
                {
                    models = new AchPermissionInputViewModels()
                    {
                        TId= fgp.Tid,
                        Id = fgp.Id,
                        AchId = fgp.FgpId,
                        Name = fgp.FgpName,
                        ProName = fgp.FgpProname,
                        ProType= fgp.FgpProtype,
                        Pruid= fgp.FgpPruid,
                        Value= fgp.FgpValue,
                        Imageurl = fgp.FgpImageurl,
                        Num = fgp.FgpNum,
                        Code = null,
                        OrgId = fgp.FgpOrgid,
                        IsButton = false,
                        Pid = ""
                    };
                }
                #endregion
                #region 第二级菜单
                var fup = (await _achFupServices.Query(a => a.FupId == achid)).FirstOrDefault();
                if (fup != null)
                {
                    models = new AchPermissionInputViewModels()
                    {
                        TId = fup.Tid,
                        Id = fup.Id,
                        AchId = fup.FupId,
                        Name = fup.FupName,
                        ProName = null,
                        Code = null,
                        OrgId = null,
                        IsButton = false,
                        Value = fup.FupValue,
                        Imageurl = fup.FupImageurl,
                        Num = fup.FupNum,
                        Pid = fup.FupFgpid
                    };
                }
                #endregion
                #region 第三级菜单
                var fun = (await _achFunServices.Query(a => a.FunId == achid)).FirstOrDefault();
                if (fun != null)
                {
                    models = new AchPermissionInputViewModels()
                    {
                        TId = fun.Tid,
                        Id = fun.Id,
                        AchId = fun.FunId,
                        Name = fun.FunName,
                        Code = fun.FunUrl,
                        OrgId = null,
                        IsButton = false,
                        Value = fun.FunValue,
                        Imageurl = fun.FunImageurl,
                        Num = fun.FunNum,
                        Pid = fun.FunFupid
                    };
                }
                #endregion
                #region 第四级菜单
                var btn = (await _achBtnServices.Query(a => a.BtnId == achid)).FirstOrDefault();
                if (btn != null)
                {
                    models = new AchPermissionInputViewModels()
                    {
                        TId = btn.Tid,
                        Id = btn.Id,
                        AchId = btn.BtnId,
                        Name = btn.BtnName,
                        ProName = null,
                        Code = null,
                        OrgId = null,
                        IsButton = true,
                        Value = btn.BtnValue,
                        ShowName = btn.BtnShowName,
                        Num = btn.BtnNum,
                        Pid = btn.BtnFunId
                    };
                }
                #endregion
            }
 
            var data = new MessageModel<AchPermissionInputViewModels> { response = models, msg = "", success = true };
            return data;

        }


        /// <summary>
        /// 根据菜单achid编码删除菜单信息
        /// </summary>
        /// <param name="achid"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<MessageModel<string>> Delete(string achid)
        {
            var data = new MessageModel<string>();
            if (!string.IsNullOrEmpty(achid))
            {

                #region 第一级菜单
                var fgp = (await _achFgpServices.Query(a => a.FgpId == achid)).FirstOrDefault();
                if (fgp != null)
                {
                    data.success = await _achFgpServices.DeleteById(fgp.Id);
                    if (data.success)
                    {
                        data.msg = "操作成功";
                    }
                }
                #endregion
                #region 第二级菜单
                var fup = (await _achFupServices.Query(a => a.FupId == achid)).FirstOrDefault();
                if (fup != null)
                {
                    data.success = await _achFupServices.DeleteById(fup.Id);
                    if (data.success)
                    {
                        data.msg = "操作成功";
                    }
                }
                #endregion
                #region 第三级菜单
                var fun = (await _achFunServices.Query(a => a.FunId == achid)).FirstOrDefault();
                if (fun != null)
                {
                    data.success = await _achFunServices.DeleteById(fun.Id);
                    if (data.success)
                    {
                        data.msg = "操作成功";
                    }
                }
                #endregion
                #region 第四级菜单
                var btn = (await _achBtnServices.Query(a => a.BtnId == achid)).FirstOrDefault();
                if (btn != null)
                {
                    data.success = await _achBtnServices.DeleteById(btn.Id);
                    if (data.success)
                    {
                        data.msg = "操作成功";
                    }
                }
                #endregion
            }
            return data;
        }


        /// <summary>
        /// 通过角色配置获取菜单
        /// </summary>
        /// <param name="rolid"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<MessageModel<AssignStringShow>> GetAchPermissionIdByRoleId(string rolid)
        {
            var data = new MessageModel<AssignStringShow>();

            var rmps = await _achRacServices.Query(d => d.RacRolid == rolid);
            var permissionTrees = (from child in rmps
                                   select child.RacBtnid).ToList();

            data.success = true;
            if (data.success)
            {
                data.response = new AssignStringShow()
                {
                    permissionids = permissionTrees
                };
                data.msg = "获取成功";
            }

            return data;
        }


        /// <summary>
        /// 分配角色按钮
        /// </summary>
        /// <param name="assignView"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<MessageModel<string>> AssignAch([FromBody] AssignAchPermissionView assignView)
        {
            var data = new MessageModel<string>();
            try
            {
                if (!string.IsNullOrEmpty(assignView.rolid))
                {
                    data.success = true;
                    foreach (var item in assignView.pids)
                    {
                        var model = (await _achRacServices.Query(p => p.RacBtnid == item && p.RacRolid == assignView.rolid&&p.Tid== assignView.tid)).FirstOrDefault();

                        if (model != null)
                        {
                            await _achRacServices.Delete(p => p.RacBtnid == item && p.RacRolid == assignView.rolid && p.Tid == assignView.tid);
                        }
                        //根据角色编号，获取客户id
                        AchRac roleModulePermission = new AchRac()
                        {
                            RacRolid = assignView.rolid,
                            RacBtnid = item,
                            Tid = assignView.tid
                        };
                        await _achRacServices.Add(roleModulePermission);
                    }
                    data.success = true;
                    if (data.success)
                    {
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
        /// 导出
        /// </summary>
        /// <param name="id">用户id</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<MessageModel<string>> Export(int id = 0)
        {
            var result = await _npoiWordExportServices.SaveWordFile("", "AchFgp", "AchFup","AchFun","AchBtn", id);

            return new MessageModel<string>()
            {
                msg = "导出成功",
                success = true,
                response = result,
            };
        }


    }
}
