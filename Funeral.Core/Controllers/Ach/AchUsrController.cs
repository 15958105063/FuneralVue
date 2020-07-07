
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
    /// 用户配置
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]

    public class AchUsrController : ControllerBase
    {
        private readonly IAchUsrServices _achUsrServices;
        private readonly IMapper _mapper;
        readonly IUser _user;
        public AchUsrController(IUser user, IMapper mapper,IAchUsrServices achUsrServices) {
            this._achUsrServices = achUsrServices;
            this._mapper = mapper;
            this._user = user;
        }


        /// <summary>
        /// 配置用户列表分页查询
        /// </summary>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <param name="key">关键字</param>
        /// <param name="orderby">排序</param>
        ///  <param name="id">客户ID</param>
        /// <returns></returns>

        [HttpGet]
        [AllowAnonymous]
        public async Task<MessageModel<PageModel<AchUsr>>> GetAchUsrListByPage(int pageindex = 1, int pagesize = 50, string orderby = "UsrId desc", string key = "",int id=1)
        {
            Expression<Func<AchUsr, bool>> whereExpression = a => (a.UsrId != "" && a.UsrId != null&&a.Tid==id);
            var pageModelBlog = await _achUsrServices.QueryPage(whereExpression, pageindex, pagesize, orderby);
            PageModel<AchUsr> querymodel = _mapper.Map<PageModel<AchUsr>>(pageModelBlog);
            return new MessageModel<PageModel<AchUsr>>()
            {
                msg = "获取成功",
                success = querymodel.dataCount >= 0,
                response = querymodel
            };
        }



        /// <summary>
        /// 根据id获取配置用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<MessageModel<AchUsr>> GetById(int id)
        {
            //先根据关联表获取角色ID，再循环获取角色name
            var model = (await _achUsrServices.Query(x => x.Id == id)).FirstOrDefault();
            var data = new MessageModel<AchUsr> { response = model, msg = "", success = true };
            return data;
        }



        /// <summary>
        /// 新增/更新配置用户信息
        /// </summary>
        /// <param name="models">机构信息实体</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<MessageModel<string>> Post([FromBody] AchUsr models)
        {
            var data = new MessageModel<string>();

            if (models.Id>0)
            {
                //更新
                models.ModifyBy = _user.ID.ToString();
                models.ModifyBy = _user.Name;
                models.ModifyTime = DateTime.Now;
                data.success = await _achUsrServices.Update(models);
                if (data.success)
                {
                    data.msg = "更新成功";
                    data.response = models?.UsrId.ObjToString();
                }
            }
            else
            {
                //新增
                models.CreateBy = _user.ID.ToString();
                models.CreateBy = _user.Name;
                var id = (await _achUsrServices.Add(models));
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
        /// 删除配置用户信息
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<MessageModel<string>> Delete(string id)
        {
            var data = new MessageModel<string>();
            if (!string.IsNullOrEmpty(id))
            {
                data.success = await _achUsrServices.DeleteById(id);
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
        /// <param name="id">用户id</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<MessageModel<string>> Export(int id=0)
        {
            var result = await _achUsrServices.SaveWordFile("", "AchUsr", id);

            return new MessageModel<string>()
            {
                msg = "导出成功",
                success = true,
                response=result,
            };
        }
    }
}
