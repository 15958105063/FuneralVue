using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Funeral.Core.Common.HttpContextUser;
using Funeral.Core.IServices;
using Funeral.Core.Model;
using Funeral.Core.Model.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Funeral.Core.Controllers
{
    /// <summary>
    /// 接口管理
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    //[Authorize(Permissions.Name)]
    [AllowAnonymous]

    public class ModuleController : ControllerBase
    {
        readonly IModuleServices _moduleServices;
        readonly IUser _user;

       
        public ModuleController(IModuleServices moduleServices, IUser user)
        {
            _moduleServices = moduleServices;
            _user = user;
        }

        /// <summary>
        /// 获取全部接口api（分页）
        /// </summary>
        /// <param name="page"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        // GET: api/User
        [HttpGet]
        public async Task<MessageModel<PageModel<Modules>>> Get(int page = 1, string key = "")
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
            {
                key = "";
            }
            int intPageSize = 50;

            Expression<Func<Modules, bool>> whereExpression = a => a.IsDeleted != true && (a.Name != null && a.Name.Contains(key));

            var data = await _moduleServices.QueryPage(whereExpression, page, intPageSize, " Id desc ");

            return new MessageModel<PageModel<Modules>>()
            {
                msg = "获取成功",
                success = data.dataCount >= 0,
                response = data
            };
        }



        /// <summary>
        /// 获取全部接口api（登录用）
        /// </summary>
        /// <returns></returns>
        // GET: api/Topic
        [HttpGet]
        public async Task<MessageModel<List<Modules>>> GetModuleAll()
        {
            var data = new MessageModel<List<Modules>> { response = await _moduleServices.Query() };
            if (data.response != null)
            {
                data.success = true;
                data.msg = "";
            }
            return data;
        }


        /// <summary>
        /// 添加一条接口信息
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        // POST: api/User
        [HttpPost]
        public async Task<MessageModel<string>> Post([FromBody] Modules module)
        {
            var data = new MessageModel<string>();

            module.CreateId = _user.ID;
            module.CreateBy = _user.Name;

            var id = (await _moduleServices.Add(module));
            data.success = id > 0;
            if (data.success)
            {
                data.response = id.ObjToString();
                data.msg = "添加成功";
            }

            return data;
        }

        /// <summary>
        /// 更新接口信息
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        // PUT: api/User/5
        [HttpPut]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<MessageModel<string>> Put([FromBody] Modules module)
        {
            var data = new MessageModel<string>();
            if (module != null && module.Id > 0)
            {
                data.success = await _moduleServices.Update(module);
                if (data.success)
                {
                    data.msg = "更新成功";
                    data.response = module?.Id.ObjToString();
                }
            }

            return data;
        }

        /// <summary>
        /// 删除一条接口
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE: api/ApiWithActions/5
        [HttpDelete]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<MessageModel<string>> Delete(int id)
        {
            var data = new MessageModel<string>();
            if (id > 0)
            {
                var userDetail = await _moduleServices.QueryById(id);
                userDetail.IsDeleted = true;
                data.success = await _moduleServices.Update(userDetail);
                if (data.success)
                {
                    data.msg = "删除成功";
                    data.response = userDetail?.Id.ObjToString();
                }
            }

            return data;
        }
    }
}
