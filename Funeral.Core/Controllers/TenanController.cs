using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Funeral.Core.Common.HttpContextUser;
using Funeral.Core.IServices;
using Funeral.Core.Model;
using Funeral.Core.Model.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Funeral.Core.Controllers
{

    /// <summary>
    /// 客户管理
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]

    public class TenanController : ControllerBase
    {

        readonly ITenanServices _tenanServices;
        readonly IUser _user;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="tenanServices"></param>
        public TenanController(ITenanServices tenanServices, IUser user)
        {
            _tenanServices = tenanServices;
            _user = user;
        }


        /// <summary>
        /// 获取客户列表
        /// </summary>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <param name="key">客户名称</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Get")]
        //[AllowAnonymous]
        [Authorize(Permissions.Name)]
        //[ApiExplorerSettings(IgnoreApi = true)]
        public async Task<MessageModel<PageModel<Tenan>>> Get(int pageindex = 1,int pagesize=50, string key = "")
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
            {
                key = "";
            }

            var data = await _tenanServices.QueryPage(a => a.Enabled && (a.TenanName != null && a.TenanName.Contains(key)), pageindex, pagesize, " Id desc ");

            return new MessageModel<PageModel<Tenan>>()
            {
                msg = "获取成功",
                success = data.dataCount >= 0,
                response = data
            };

        }


        /// <summary>
        /// 获取所有客户信息（登录用）
        /// </summary>
        /// <returns></returns>
        // GET: api/Topic
        [HttpGet]
        [Route("GetTenanAll")]
        public async Task<MessageModel<List<Tenan>>> GetAll ()
        {
            var data = new MessageModel<List<Tenan>> { response = await _tenanServices.GetAll() };
            if (data.response != null)
            {
                data.success = true;
                data.msg = "";
            }
            return data;
        }


        /// <summary>
        /// 新增/更新客户
        /// </summary>
        /// <param name="tenan">客户信息实体</param>
        /// <returns></returns>
        [HttpPost]
        [Route("Post")]
        public async Task<MessageModel<string>> Post([FromBody] Tenan tenan)
        {
            var data = new MessageModel<string>();

            if (tenan != null && tenan.Id > 0)
            {
                //更新
                tenan.ModifyBy = _user.ID.ToString();
                tenan.ModifyBy = _user.Name;
                data.success = await _tenanServices.Update(tenan);
                if (data.success)
                {
                    data.msg = "更新成功";
                    data.response = tenan?.Id.ObjToString();
                }
            }
            else {
                //新增
                tenan.CreateBy = _user.ID.ToString();
                tenan.CreateBy = _user.Name;
                var id = (await _tenanServices.Add(tenan));
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
        /// 禁用客户
        /// </summary>
        /// <param name="id">客户ID</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("Delete")]
        public async Task<MessageModel<string>> Delete(int id)
        {
            var data = new MessageModel<string>();
            if (id > 0)
            {
                var userDetail = await _tenanServices.QueryById(id);
                userDetail.Enabled = false;
                data.success = await _tenanServices.Update(userDetail);
                if (data.success)
                {
                    data.msg = "禁用成功";
                    data.response = userDetail?.Id.ObjToString();
                }
            }

            return data;
        }

        /// <summary>
        /// 根据ID获取客户信息
        /// </summary>
        /// <param name="tid"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetById")]
        public async Task<MessageModel<Tenan>> GetById(int tid) {

            var data = new MessageModel<Tenan> { response = await _tenanServices.QueryById(tid) };
            if (data.response != null)
            {
                data.success = true;
                data.msg = "";
            }
            return data;
        }

    }
}
