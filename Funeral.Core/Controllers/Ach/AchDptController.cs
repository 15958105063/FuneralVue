
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
    /// 部门配置
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AchDptController : Controller
    {

        readonly IUser _user;
        readonly IAchDptServices _achDptServices;
        private readonly INpoiWordExportServices _npoiWordExportServices;
        public AchDptController(INpoiWordExportServices npoiWordExportServices, IUser user, IAchDptServices achDptServices) {
            this._user = user;
            this._achDptServices = achDptServices;
            this._npoiWordExportServices = npoiWordExportServices;
        }

        /// <summary>
        /// 根据客户ID获取部门信息列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<MessageModel<List<KeyValue>>> Get(int id)
        {
            var list = await _achDptServices.Query(a => a.Tid == id);
            var returnlist = (from child in list
                              orderby child.DptId
                              select new KeyValue
                              {
                                  Key = child.DptId,
                                  Value = child.DptName,
                              }).ToList();
            var data = new MessageModel<List<KeyValue>> { };
            data.success = true;
            data.msg = "";
            data.response = returnlist;
            return data;
        }

        /// <summary>
        /// 根据id获取部门信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<MessageModel<AchDpt>> GetById(int id)
        {
            //先根据关联表获取角色ID，再循环获取角色name
            var model = (await _achDptServices.Query(x => x.Id == id)).FirstOrDefault();
            var data = new MessageModel<AchDpt> { response = model, msg = "", success = true };
            return data;
        }


        /// <summary>
        /// 新增/更新部门信息
        /// </summary>
        /// <param name="models">部门信息实体</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<MessageModel<string>> Post([FromBody] AchDpt models)
        {
            var data = new MessageModel<string>();

            if (models.Id > 0)
            {
         
                //更新
                models.ModifyBy = _user.ID.ToString();
                models.ModifyBy = _user.Name;
                models.ModifyTime = DateTime.Now;
                data.success = await _achDptServices.Update(models);
                if (data.success)
                {
                    data.msg = "更新成功";
                    data.response = models?.DptId.ObjToString();
                }
            }
            else
            {
                //新增
                models.CreateBy = _user.ID.ToString();
                models.CreateBy = _user.Name;
                var id = (await _achDptServices.Add(models));
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
        /// 分页查询部门信息
        /// </summary>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <param name="orderby"></param>
        /// <param name="key"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<MessageModel<PageModel<AchDpt>>> GetAchDptListByPage(int pageindex = 1, int pagesize = 50, string orderby = "DptId desc", string key = "", int id = 1)
        {
            Expression<Func<AchDpt, bool>> whereExpression = a => (a.DptId != "" && a.DptId != null && a.Tid == id);
            var pageModelBlog = await _achDptServices.QueryPage(whereExpression, pageindex, pagesize, orderby);
            //PageModel<AchDpt> querymodel = _mapper.Map<PageModel<AchDpt>>(pageModelBlog);
            return new MessageModel<PageModel<AchDpt>>()
            {
                msg = "获取成功",
                success = pageModelBlog.dataCount >= 0,
                response = pageModelBlog
            };
        }


        /// <summary>
        /// 删除部门信息
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<MessageModel<string>> Delete(int id)
        {
            var data = new MessageModel<string>();
            if (id > 0)
            {

                data.success = await _achDptServices.DeleteById(id);
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
            var result = await _npoiWordExportServices.SaveWordFile("", "AchDpt", id);

            return new MessageModel<string>()
            {
                msg = "导出成功",
                success = true,
                response=result,
            };
        }

    }
}
