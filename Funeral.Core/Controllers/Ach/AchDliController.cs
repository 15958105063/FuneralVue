
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
    /// 列表配置
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AchDliController : Controller
    {

        readonly IUser _user;
        readonly IAchDliServices _achDliServices;

        public AchDliController(IUser user, IAchDliServices achDliServices) {
            this._user = user;
            this._achDliServices = achDliServices;;
        }

        /// <summary>
        /// 根据客户ID获取列表信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<MessageModel<List<KeyValue>>> Get(int id)
        {
            var list = await _achDliServices.Query(a => a.Tid == id);
            var returnlist = (from child in list
                              orderby child.DliId
                              select new KeyValue
                              {
                                  Key = child.DliId,
                                  Value = child.DLIValue,
                              }).ToList();
            var data = new MessageModel<List<KeyValue>> { };
            data.success = true;
            data.msg = "";
            data.response = returnlist;
            return data;
        }

        /// <summary>
        /// 根据id获取列表信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<MessageModel<AchDli>> GetById(int id)
        {
            var model = (await _achDliServices.Query(x => x.Id == id)).FirstOrDefault();
            var data = new MessageModel<AchDli> { response = model, msg = "", success = true };
            return data;
        }


        /// <summary>
        /// 新增/更新列表信息
        /// </summary>
        /// <param name="models">列表信息实体</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<MessageModel<string>> Post([FromBody] AchDli models)
        {
            var data = new MessageModel<string>();

            if (models.Id > 0)
            {
         
                //更新
                models.ModifyBy = _user.ID.ToString();
                models.ModifyBy = _user.Name;
                models.ModifyTime = DateTime.Now;
                data.success = await _achDliServices.Update(models);
                if (data.success)
                {
                    data.msg = "更新成功";
                    data.response = models?.DliId.ObjToString();
                }
            }
            else
            {
                //新增
                models.CreateBy = _user.ID.ToString();
                models.CreateBy = _user.Name;
                models.DliProname = "Funeral";
                models.DliProtype = "WebPc";

                var id = (await _achDliServices.Add(models));
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
        /// 分页查询列表信息
        /// </summary>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <param name="orderby"></param>
        /// <param name="key"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<MessageModel<PageModel<AchDli>>> GetAchDliListByPage(int pageindex = 1, int pagesize = 50, string orderby = "DliId desc", string key = "", int id = 1)
        {
            Expression<Func<AchDli, bool>> whereExpression = a => (a.DliId != "" && a.DliId != null && a.Tid == id);
            var pageModelBlog = await _achDliServices.QueryPage(whereExpression, pageindex, pagesize, orderby);
            //PageModel<AchDpt> querymodel = _mapper.Map<PageModel<AchDpt>>(pageModelBlog);
            return new MessageModel<PageModel<AchDli>>()
            {
                msg = "获取成功",
                success = pageModelBlog.dataCount >= 0,
                response = pageModelBlog
            };
        }


        /// <summary>
        /// 删除列表信息
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<MessageModel<string>> Delete(int id)
        {
            var data = new MessageModel<string>();
            if (id > 0)
            {

                data.success = await _achDliServices.DeleteById(id);
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
            var result = await _achDliServices.SaveWordFile("", "AchDli", id);

            return new MessageModel<string>()
            {
                msg = "导出成功",
                success = true,
                response=result,
            };
        }

    }
}
