
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
    /// 死亡原因小类配置
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AchCauController : Controller
    {

        readonly IUser _user;
        readonly IAchCauServices _AchCauServices;

        public AchCauController(IUser user, IAchCauServices AchCauServices) {
            this._user = user;
            this._AchCauServices = AchCauServices;;
        }

        /// <summary>
        /// 根据客户ID获取死亡原因小类信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<MessageModel<List<KeyValue>>> Get(int id)
        {
            var list = await _AchCauServices.Query(a => a.Tid == id);
            var returnlist = (from child in list
                              orderby child.CauId
                              select new KeyValue
                              {
                                  Key = child.CauId,
                                  Value = child.CauName,
                              }).ToList();
            var data = new MessageModel<List<KeyValue>> { };
            data.success = true;
            data.msg = "";
            data.response = returnlist;
            return data;
        }

        /// <summary>
        /// 根据id获取死亡原因小类信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<MessageModel<AchCau>> GetById(int id)
        {
            var model = (await _AchCauServices.Query(x => x.Id == id)).FirstOrDefault();
            var data = new MessageModel<AchCau> { response = model, msg = "", success = true };
            return data;
        }


        /// <summary>
        /// 新增/更新死亡原因小类信息
        /// </summary>
        /// <param name="models">编码信息实体</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<MessageModel<string>> Post([FromBody] AchCau models)
        {
            var data = new MessageModel<string>();

            if (models.Id > 0)
            {
         
                //更新
                models.ModifyBy = _user.ID.ToString();
                models.ModifyBy = _user.Name;
                models.ModifyTime = DateTime.Now;
                data.success = await _AchCauServices.Update(models);
                if (data.success)
                {
                    data.msg = "更新成功";
                    data.response = models?.CauId.ObjToString();
                }
            }
            else
            {
                //新增
                models.CreateBy = _user.ID.ToString();
                models.CreateBy = _user.Name;
                var id = (await _AchCauServices.Add(models));
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
        /// 分页查询死亡原因小类信息
        /// </summary>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <param name="orderby"></param>
        /// <param name="key"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<MessageModel<PageModel<AchCau>>> GetAchCauListByPage(int pageindex = 1, int pagesize = 50, string orderby = "CauId desc", string key = "", int id = 1)
        {
            Expression<Func<AchCau, bool>> whereExpression = a => (a.Tid == id);
            var pageModelBlog = await _AchCauServices.QueryPage(whereExpression, pageindex, pagesize, orderby);
            //PageModel<AchDpt> querymodel = _mapper.Map<PageModel<AchDpt>>(pageModelBlog);
            return new MessageModel<PageModel<AchCau>>()
            {
                msg = "获取成功",
                success = pageModelBlog.dataCount >= 0,
                response = pageModelBlog
            };
        }


        /// <summary>
        /// 删除死亡原因小类信息
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<MessageModel<string>> Delete(int id)
        {
            var data = new MessageModel<string>();
            if (id > 0)
            {

                data.success = await _AchCauServices.DeleteById(id);
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
            var result = await _AchCauServices.SaveWordFile("", "AchCau", id);

            return new MessageModel<string>()
            {
                msg = "导出成功",
                success = true,
                response=result,
            };
        }

    }
}
