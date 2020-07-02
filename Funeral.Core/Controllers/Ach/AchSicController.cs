
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
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Funeral.Core.Controllers
{
    /// <summary>
    /// 服务种类配置
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]

    public class AchSicController : ControllerBase
    {
        private readonly INpoiWordExportServices _npoiWordExportServices;
        private readonly IAchSicServices _achSicServices;
        private readonly IMapper _mapper;
        readonly IUser _user;
        public AchSicController(INpoiWordExportServices npoiWordExportServices,IUser user, IMapper mapper,IAchSicServices achSicServices) {
            this._npoiWordExportServices = npoiWordExportServices;
            this._achSicServices = achSicServices;
            this._mapper = mapper;
            this._user = user;
        }


        /// <summary>
        /// 服务种类列表分页查询
        /// </summary>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <param name="key">关键字</param>
        /// <param name="orderby">排序</param>
        ///  <param name="id">客户ID</param>
        /// <returns></returns>

        [HttpGet]
        public async Task<MessageModel<PageModel<AchSic>>> GetAchSicListByPage(int pageindex = 1, int pagesize = 50, string orderby = "SicId desc", string key = "",int id=1)
        {
            Expression<Func<AchSic, bool>> whereExpression = a => (a.SicId != "" && a.SicId != null&&a.Tid==id);
            var pageModelBlog = await _achSicServices.QueryPage(whereExpression, pageindex, pagesize, orderby);
            PageModel<AchSic> querymodel = _mapper.Map<PageModel<AchSic>>(pageModelBlog);
            return new MessageModel<PageModel<AchSic>>()
            {
                msg = "获取成功",
                success = querymodel.dataCount >= 0,
                response = querymodel
            };
        }

        /// <summary>
        /// 根据客户id，查询服务种类信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<MessageModel<List<KeyValue>>> Get(int id)
        {


            var list = await _achSicServices.Query(a => a.Tid == id);
            var returnlist = (from child in list
                              orderby child.SicId
                              select new KeyValue
                              {
                                  Key = child.SicId,
                                  Value = child.SicName,

                              }).ToList();
            var data = new MessageModel<List<KeyValue>> { };

            data.success = true;
            data.msg = "";
            data.response = returnlist;
            return data;
        }


        /// <summary>
        /// 根据id获取服务种类信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<MessageModel<AchSic>> GetById(int id)
        {
            //先根据关联表获取角色ID，再循环获取角色name
            var model = (await _achSicServices.Query(x => x.Id == id)).FirstOrDefault();
            var data = new MessageModel<AchSic> { response = model, msg = "", success = true };
            return data;
        }



        /// <summary>
        /// 新增/更新服务种类信息
        /// </summary>
        /// <param name="models">机构信息实体</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<MessageModel<string>> Post([FromBody] AchSic models)
        {
            var data = new MessageModel<string>();

            if (models.Id>0)
            {
                //更新
                models.ModifyBy = _user.ID.ToString();
                models.ModifyBy = _user.Name;
                models.ModifyTime = DateTime.Now;
                data.success = await _achSicServices.Update(models);
                if (data.success)
                {
                    data.msg = "更新成功";
                    data.response = models?.SicId.ObjToString();
                }
            }
            else
            {
                //新增
                models.CreateBy = _user.ID.ToString();
                models.CreateBy = _user.Name;
                var id = (await _achSicServices.Add(models));
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
        /// 删除服务种类信息
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<MessageModel<string>> Delete(string id)
        {
            var data = new MessageModel<string>();
            if (!string.IsNullOrEmpty(id))
            {
                data.success = await _achSicServices.DeleteById(id);
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
        public async Task<MessageModel<string>> Export(int id=0)
        {
            var result = await _npoiWordExportServices.SaveWordFile("", "AchSic", id);

            return new MessageModel<string>()
            {
                msg = "导出成功",
                success = true,
                response=result,
            };
        }
    }
}
