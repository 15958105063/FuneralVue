
using AutoMapper;
using Funeral.Core.Common.HttpContextUser;
using Funeral.Core.IServices;
using Funeral.Core.Model;
using Funeral.Core.Model.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Funeral.Core.Controllers
{
    /// <summary>
    /// 厂家配置
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]

    public class AchVdrController : ControllerBase
    {
        private readonly INpoiWordExportServices _npoiWordExportServices;
        private readonly IAchVdrServices _achVdrServices;
        private readonly IMapper _mapper;
        readonly IUser _user;
        public AchVdrController(INpoiWordExportServices npoiWordExportServices,IUser user, IMapper mapper,IAchVdrServices achVdrServices) {
            this._npoiWordExportServices = npoiWordExportServices;
            this._achVdrServices = achVdrServices;
            this._mapper = mapper;
            this._user = user;
        }


        /// <summary>
        /// 配置厂家列表分页查询
        /// </summary>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <param name="key">关键字</param>
        /// <param name="orderby">排序</param>
        ///  <param name="id">客户ID</param>
        /// <returns></returns>

        [HttpGet]
        [AllowAnonymous]
        public async Task<MessageModel<PageModel<AchVdr>>> GetAchVdrListByPage(int pageindex = 1, int pagesize = 50, string orderby = "VdrId desc", string key = "",int id=1)
        {
            Expression<Func<AchVdr, bool>> whereExpression = a => (a.VdrId != "" && a.VdrId != null&&a.Tid==id);
            var pageModelBlog = await _achVdrServices.QueryPage(whereExpression, pageindex, pagesize, orderby);
            PageModel<AchVdr> querymodel = _mapper.Map<PageModel<AchVdr>>(pageModelBlog);
            return new MessageModel<PageModel<AchVdr>>()
            {
                msg = "获取成功",
                success = querymodel.dataCount >= 0,
                response = querymodel
            };
        }



        /// <summary>
        /// 根据id获取配置厂家信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<MessageModel<AchVdr>> GetById(string id)
        {
            //先根据关联表获取角色ID，再循环获取角色name
            var model = (await _achVdrServices.Query(x => x.VdrId == id)).FirstOrDefault();
            var data = new MessageModel<AchVdr> { response = model, msg = "", success = true };
            return data;
        }



        /// <summary>
        /// 新增/更新配置厂家信息
        /// </summary>
        /// <param name="models">厂家信息实体</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<MessageModel<string>> Post([FromBody] AchVdr models)
        {
            var data = new MessageModel<string>();

            if (!string.IsNullOrEmpty(models.VdrId))
            {
                //更新
                models.ModifyBy = _user.ID.ToString();
                models.ModifyBy = _user.Name;
                models.ModifyTime = DateTime.Now;
                data.success = await _achVdrServices.Update(models);
                if (data.success)
                {
                    data.msg = "更新成功";
                    data.response = models?.VdrId.ObjToString();
                }
            }
            else
            {
                //新增
                models.CreateBy = _user.ID.ToString();
                models.CreateBy = _user.Name;
                var id = (await _achVdrServices.Add(models));
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
        /// 删除配置厂家信息
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<MessageModel<string>> Delete(int id)
        {
            var data = new MessageModel<string>();
            if (id > 0)
            {
                data.success = await _achVdrServices.DeleteById(id);
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
        /// <param name="id">厂家id</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<MessageModel<string>> Export(int id=0)
        {
            bool result = await _npoiWordExportServices.SaveWordFile("", "AchVdr", id);

            return new MessageModel<string>()
            {
                msg = "导出成功",
                success = result
            };
        }
    }
}
