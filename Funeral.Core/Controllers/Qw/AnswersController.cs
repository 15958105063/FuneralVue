
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
    /// 回答管理
    /// 这里进行优化，业务部分全部在服务层处理
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AnswersController : ControllerBase
    {
        private readonly IAnswersServices _answersServices;
        private readonly IMapper _mapper;

        public AnswersController(IAnswersServices answersServices) {
            this._answersServices = answersServices;
        }

        /// <summary>
        /// 新增/编辑回答信息
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<MessageModel<string>> Post([FromBody] AnswersDto models)
        {
            return new MessageModel<string>()
            {
                msg = "操作成功",
                success = await _answersServices.PostAnswersInfo(models),
                response = "",
            };
        }

    }
}
