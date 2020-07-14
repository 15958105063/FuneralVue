
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
    /// 列表表头配置
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AchDlxController : Controller
    {

        readonly IUser _user;
        readonly IAchDlxServices _achDlxServices;

        public AchDlxController(IUser user, IAchDlxServices achDlxServices) {
            this._user = user;
            this._achDlxServices = achDlxServices;;
        }

        /// <summary>
        /// 根据客户ID获取列表表头信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<MessageModel<List<KeyValue>>> Get(int id)
        {
            var list = await _achDlxServices.Query(a => a.Tid == id);
            var returnlist = (from child in list
                              orderby child.DlxId
                              select new KeyValue
                              {
                                  Key = child.DlxId,
                                  Value = child.DlxValue,
                              }).ToList();
            var data = new MessageModel<List<KeyValue>> { };
            data.success = true;
            data.msg = "";
            data.response = returnlist;
            return data;
        }

        /// <summary>
        /// 根据id获取列表表头信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<MessageModel<AchDlx>> GetById(int id)
        {
            var model = (await _achDlxServices.Query(x => x.Id == id)).FirstOrDefault();
            var data = new MessageModel<AchDlx> { response = model, msg = "", success = true };
            return data;
        }


        /// <summary>
        /// 新增/更新列表表头信息
        /// </summary>
        /// <param name="models">列表信息实体</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<MessageModel<string>> Post([FromBody] AchDlx models)
        {
            var data = new MessageModel<string>();

            if (models.Id > 0)
            {
         
                //更新
                models.ModifyBy = _user.ID.ToString();
                models.ModifyBy = _user.Name;
                models.ModifyTime = DateTime.Now;
                data.success = await _achDlxServices.Update(models);
                if (data.success)
                {
                    data.msg = "更新成功";
                    data.response = models?.DlxId.ObjToString();
                }
            }
            else
            {
                //新增
                models.CreateBy = _user.ID.ToString();
                models.CreateBy = _user.Name;
                var id = (await _achDlxServices.Add(models));
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
        /// 分页查询列表表头信息
        /// </summary>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <param name="orderby"></param>
        /// <param name="key"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<MessageModel<PageModel<AchDlx>>> GetAchDlxListByPage(int pageindex = 1, int pagesize = 50, string orderby = "DlxId desc", string key = "", int id = 1)
        {
            Expression<Func<AchDlx, bool>> whereExpression = a => (a.DlxId != "" && a.DlxId != null && a.Tid == id);
            var pageModelBlog = await _achDlxServices.QueryPage(whereExpression, pageindex, pagesize, orderby);
            //PageModel<AchDpt> querymodel = _mapper.Map<PageModel<AchDpt>>(pageModelBlog);
            return new MessageModel<PageModel<AchDlx>>()
            {
                msg = "获取成功",
                success = pageModelBlog.dataCount >= 0,
                response = pageModelBlog
            };
        }


        /// <summary>
        /// 删除列表表头信息
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<MessageModel<string>> Delete(int id)
        {
            var data = new MessageModel<string>();
            if (id > 0)
            {

                data.success = await _achDlxServices.DeleteById(id);
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
            var result = await _achDlxServices.SaveWordFile("", "AchDlx", id);

            return new MessageModel<string>()
            {
                msg = "导出成功",
                success = true,
                response=result,
            };
        }

    }
}
