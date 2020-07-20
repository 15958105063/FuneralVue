
using AutoMapper;
using Funeral.Core.Common.HttpContextUser;
using Funeral.Core.IServices;
using Funeral.Core.Model;
using Funeral.Core.Model.Models;
using Funeral.Core.Model.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Funeral.Core.Controllers
{
    /// <summary>
    /// 问题信息管理
    /// 这里进行优化，业务部分全部在服务层处理
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        private readonly IQuestionsServices _questionsServices;
        private readonly IMapper _mapper;

        public QuestionsController(IQuestionsServices questionsServices) {
            this._questionsServices = questionsServices;
        }

        /// <summary>
        /// 新增/编辑问题信息
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<MessageModel<string>> Post([FromBody] QuestionsDto models)
        {
            return new MessageModel<string>()
            {
                msg = "操作成功",
                success = await _questionsServices.PostQuestionInfo(models),
                response = "",
            };
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <param name="orderby"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<MessageModel<PageModel<QuestionsDto>>> GetListByPage(int pageindex = 1, int pagesize = 50, string orderby = "", string key = "")
        {
            return new MessageModel<PageModel<QuestionsDto>>()
            {
                msg = "获取成功",
                success = true,
                response = await _questionsServices.GetListByPage(pageindex, pagesize, orderby, key)
            };
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<MessageModel<string>> Delete(int id)
        {
            var data = new MessageModel<string>();
            if (id > 0)
            {

                data.success = await _questionsServices.DeleteById(id);
                if (data.success)
                {
                    data.msg = "操作成功";
                }
            }

            return data;
        }



    }
}
