
using AutoMapper;
using Funeral.Core.Common.HttpContextUser;
using Funeral.Core.IServices;
using Funeral.Core.Model;
using Funeral.Core.Model.Models;
using Funeral.Core.Model.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Funeral.Core.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Permissions.Name)]
    public class AchOrgController : ControllerBase
    {
        private readonly IAchOrgServices _achOrgServices;
        private readonly IMapper _mapper;
        readonly IUser _user;
        public AchOrgController(IUser user, IMapper mapper,IAchOrgServices achOrgServices) {
            this._achOrgServices = achOrgServices;
            this._mapper = mapper;
            this._user = user;
        }


        /// <summary>
        /// 机构列表分页查询
        /// </summary>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <param name="key">关键字</param>
        /// <param name="orderby">排序</param>
        /// <returns></returns>

        [Route("GetAchOrgListByPage")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<MessageModel<PageModel<AchOrgInputViewModels>>> GetAchOrgListByPage(int pageindex = 1, int pagesize = 50, string orderby = "OrgId desc", string key = "")
        {
            Expression<Func<AchOrg, bool>> whereExpression = a => (a.OrgId != "" && a.OrgId != null);
            var pageModelBlog = await _achOrgServices.QueryPage(whereExpression, pageindex, pagesize, orderby);
            PageModel<AchOrgInputViewModels> querymodel = _mapper.Map<PageModel<AchOrgInputViewModels>>(pageModelBlog);
            return new MessageModel<PageModel<AchOrgInputViewModels>>()
            {
                msg = "获取成功",
                success = querymodel.dataCount >= 0,
                response = querymodel
            };
        }


        /// <summary>
        /// 新增/更新机构信息
        /// </summary>
        /// <param name="models">机构信息视图模型</param>
        /// <returns></returns>
        [HttpPost]
        [Route("Post")]
        public async Task<MessageModel<string>> Post([FromBody] AchOrg models)
        {
            var data = new MessageModel<string>();

            if (!string.IsNullOrEmpty(models.OrgId))
            {
                //更新
                models.ModifyBy = _user.ID.ToString();
                models.ModifyBy = _user.Name;
                models.ModifyTime = DateTime.Now;
                data.success = await _achOrgServices.Update(models);
                if (data.success)
                {
                    data.msg = "更新成功";
                    data.response = models?.OrgId.ObjToString();
                }
            }
            else
            {
                //新增
                models.CreateBy = _user.ID.ToString();
                models.CreateBy = _user.Name;
                var id = (await _achOrgServices.Add(models));
                data.success = id > 0;
                if (data.success)
                {
                    data.response = id.ObjToString();
                    data.msg = "添加成功";
                }
            }
            return data;
        }

    }
}
