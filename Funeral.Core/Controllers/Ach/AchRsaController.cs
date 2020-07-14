
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

namespace Funeral.Core.Controllers.Ach
{

    /// <summary>
    /// 资源安排表配置
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AchRsaController : Controller
    {

        readonly IUser _user;
        readonly IAchRsaServices _AchRsaServices;

        public AchRsaController(IUser user, IAchRsaServices AchRsaServices)
        {
            this._user = user;
            this._AchRsaServices = AchRsaServices; ;
        }



        /// <summary>
        /// 根据id获取资源表信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<MessageModel<AchRsa>> GetById(int id)
        {
            var model = (await _AchRsaServices.Query(x => x.Id == id)).FirstOrDefault();
            var data = new MessageModel<AchRsa> { response = model, msg = "", success = true };
            return data;
        }


        /// <summary>
        /// 新增/更新资源表信息
        /// </summary>
        /// <param name="models">资源表信息实体</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<MessageModel<string>> Post([FromBody] AchRsa models)
        {
            var data = new MessageModel<string>();

            if (models.Id > 0)
            {

                //更新
                models.ModifyBy = _user.ID.ToString();
                models.ModifyBy = _user.Name;
                models.ModifyTime = DateTime.Now;
                data.success = await _AchRsaServices.Update(models);
                if (data.success)
                {
                    data.msg = "更新成功";
                    data.response = models?.RsaId.ObjToString();
                }
            }
            else
            {
                //新增
                models.CreateBy = _user.ID.ToString();
                models.CreateBy = _user.Name;
                var id = (await _AchRsaServices.Add(models));
                data.success = id > 0;
                if (data.success)
                {
                    data.response = id.ObjToString();
                    data.msg = "添加成功";
                }
            }
            return data;
        }


        /// <summary>
        /// 分页查询资源表信息
        /// </summary>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <param name="orderby"></param>
        /// <param name="key"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<MessageModel<PageModel<AchRsa>>> GetAchRsaListByPage(int pageindex = 1, int pagesize = 50, string orderby = "RsaId desc", string key = "", int id = 1)
        {
            Expression<Func<AchRsa, bool>> whereExpression = a => (a.Tid == id);
            var pageModelBlog = await _AchRsaServices.QueryPage(whereExpression, pageindex, pagesize, orderby);
            //PageModel<AchDpt> querymodel = _mapper.Map<PageModel<AchDpt>>(pageModelBlog);
            return new MessageModel<PageModel<AchRsa>>()
            {
                msg = "获取成功",
                success = pageModelBlog.dataCount >= 0,
                response = pageModelBlog
            };
        }


        /// <summary>
        /// 删除资源表信息
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<MessageModel<string>> Delete(int id)
        {
            var data = new MessageModel<string>();
            if (id > 0)
            {

                data.success = await _AchRsaServices.DeleteById(id);
                if (data.success)
                {
                    data.msg = "操作成功";
                }
            }

            return data;
        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="id">客户id</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<MessageModel<string>> Export(int id = 0)
        {
            var result = await _AchRsaServices.SaveWordFile("", "AchRsa", id);

            return new MessageModel<string>()
            {
                msg = "导出成功",
                success = true,
                response = result,
            };
        }

    }
}
