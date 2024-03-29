﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Funeral.Core.Common.Helper;
using Funeral.Core.IServices;
using Funeral.Core.Model;
using Funeral.Core.Model.Models;
using Funeral.Core.Model.ViewModels;
using Funeral.Core.SwaggerHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StackExchange.Profiling;
using static Funeral.Core.SwaggerHelper.CustomApiVersion;

namespace Funeral.Core.Controllers
{
    /// <summary>
    /// 问题/博客管理
    /// </summary>
    [Produces("application/json")]
    [Route("api/Blog")]

    public class BlogController : Controller
    {
        readonly IBlogArticleServices _blogArticleServices;
        private readonly ILogger<BlogController> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="blogArticleServices"></param>
        /// <param name="logger"></param>
        public BlogController(IBlogArticleServices blogArticleServices, ILogger<BlogController> logger)
        {
            _blogArticleServices = blogArticleServices;
            _logger = logger;
        }


        /// <summary>
        /// 获取博客列表
        /// </summary>
        /// <param name="id"></param>
        /// <param name="page"></param>
        /// <param name="bcategory"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        //[ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        //[ResponseCache(Duration = 600)]
        public async Task<MessageModel<PageModel<BlogArticle>>> Get(int id, int page = 1, string bcategory = "", string key = "")
        {
            int intPageSize = 6;
            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
            {
                key = "";
            }

            Expression<Func<BlogArticle, bool>> whereExpression = a => (a.bcategory == bcategory && a.IsDeleted == false) && ((a.btitle != null && a.btitle.Contains(key)) || (a.bcontent != null && a.bcontent.Contains(key)));

            var pageModelBlog = await _blogArticleServices.QueryPage(whereExpression, page, intPageSize, " bID desc ");

            using (MiniProfiler.Current.Step("获取成功后，开始处理最终数据"))
            {
                foreach (var item in pageModelBlog.data)
                {
                    if (!string.IsNullOrEmpty(item.bcontent))
                    {
                        item.bRemark = (HtmlHelper.ReplaceHtmlTag(item.bcontent)).Length >= 200 ? (HtmlHelper.ReplaceHtmlTag(item.bcontent)).Substring(0, 200) : (HtmlHelper.ReplaceHtmlTag(item.bcontent));
                        int totalLength = 500;
                        if (item.bcontent.Length > totalLength)
                        {
                            item.bcontent = item.bcontent.Substring(0, totalLength);
                        }
                    }
                }
            }

            return new MessageModel<PageModel<BlogArticle>>()
            {
                success = true,
                msg = "获取成功",
                response = new PageModel<BlogArticle>()
                {
                    page = page,
                    dataCount = pageModelBlog.dataCount,
                    data = pageModelBlog.data,
                    pageCount = pageModelBlog.pageCount,
                }
            };
        }


        /// <summary>
        /// 获取博客详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Authorize]
        public async Task<MessageModel<BlogViewModels>> Get(int id)
        {
            return new MessageModel<BlogViewModels>()
            {
                msg = "获取成功",
                success = true,
                response = await _blogArticleServices.GetBlogDetails(id)
            };
        }


        /// <summary>
        /// 获取详情【无权限】
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("DetailNuxtNoPer")]
        [AllowAnonymous]
        public async Task<MessageModel<BlogViewModels>> DetailNuxtNoPer(int id)
        {
            _logger.LogInformation("xxxxxxxxxxxxxxxxxxx");
            return new MessageModel<BlogViewModels>()
            {
                msg = "获取成功",
                success = true,
                response = await _blogArticleServices.GetBlogDetails(id)
            };
        }

        [HttpGet]
        [Route("GoUrl")]
        [AllowAnonymous]
        public async Task<IActionResult> GoUrl(int id)
        {
            var response = await _blogArticleServices.QueryById(id);
            if (response != null && response.bsubmitter.IsNotEmptyOrNull())
            {
                response.btraffic += 1;
                await _blogArticleServices.Update(response);
                return Redirect(response.bsubmitter);
            }

            return null;
        }

        [HttpGet]
        [Route("GetBlogsByTypesForMVP")]
        [AllowAnonymous]
        public async Task<MessageModel<List<BlogArticle>>> GetBlogsByTypesForMVP(string types = "", int id = 0)
        {
            if (types.IsNotEmptyOrNull())
            {
                var blogs = await _blogArticleServices.Query(d => d.bcategory != null && types.Contains(d.bcategory) && d.IsDeleted == false);
                return new MessageModel<List<BlogArticle>>()
                {
                    msg = "获取成功",
                    success = true,
                    response = blogs
                };
            }

            return new MessageModel<List<BlogArticle>>() { };
        }

        [HttpGet]
        [Route("GetBlogByIdForMVP")]
        [AllowAnonymous]
        public async Task<MessageModel<BlogArticle>> GetBlogByIdForMVP(int id = 0)
        {
            if (id > 0)
            {
                return new MessageModel<BlogArticle>()
                {
                    msg = "获取成功",
                    success = true,
                    response = await _blogArticleServices.QueryById(id)
                };
            }

            return new MessageModel<BlogArticle>() { };
        }

        /// <summary>
        /// 添加博客【无权限】
        /// </summary>
        /// <param name="blogArticle"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<MessageModel<string>> Post([FromBody] BlogArticle blogArticle)
        {
            var data = new MessageModel<string>();

            blogArticle.bCreateTime = DateTime.Now;
            blogArticle.bUpdateTime = DateTime.Now;
            blogArticle.IsDeleted = false;
            blogArticle.bcategory = "技术博文";

            var id = (await _blogArticleServices.Add(blogArticle));
            data.success = id > 0;
            if (data.success)
            {
                data.response = id.ObjToString();
                data.msg = "添加成功";
            }

            return data;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="blogArticle"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddForMVP")]
        [Authorize(Permissions.Name)]
        public async Task<MessageModel<string>> AddForMVP([FromBody] BlogArticle blogArticle)
        {
            var data = new MessageModel<string>();

            blogArticle.bCreateTime = DateTime.Now;
            blogArticle.bUpdateTime = DateTime.Now;
            blogArticle.IsDeleted = false;

            var id = (await _blogArticleServices.Add(blogArticle));
            data.success = id > 0;
            if (data.success)
            {
                data.response = id.ObjToString();
                data.msg = "添加成功";
            }

            return data;
        }
        /// <summary>
        /// 更新博客信息
        /// </summary>
        /// <param name="BlogArticle"></param>
        /// <returns></returns>
        // PUT: api/User/5
        [HttpPut]
        [Route("Update")]
        [Authorize(Permissions.Name)]
        public async Task<MessageModel<string>> Put([FromBody] BlogArticle BlogArticle)
        {
            var data = new MessageModel<string>();
            if (BlogArticle != null && BlogArticle.bID > 0)
            {
                var model = await _blogArticleServices.QueryById(BlogArticle.bID);

                if (model != null)
                {
                    model.btitle = BlogArticle.btitle;
                    model.bcategory = BlogArticle.bcategory;
                    model.bsubmitter = BlogArticle.bsubmitter;
                    model.bcontent = BlogArticle.bcontent;
                    model.btraffic = BlogArticle.btraffic;

                    data.success = await _blogArticleServices.Update(model);
                    if (data.success)
                    {
                        data.msg = "更新成功";
                        data.response = BlogArticle?.bID.ObjToString();
                    }
                }

            }

            return data;
        }



        /// <summary>
        /// 删除博客
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize(Permissions.Name)]
        [Route("Delete")]
        public async Task<MessageModel<string>> Delete(int id)
        {
            var data = new MessageModel<string>();
            if (id > 0)
            {
                var blogArticle = await _blogArticleServices.QueryById(id);
                blogArticle.IsDeleted = true;
                data.success = await _blogArticleServices.Update(blogArticle);
                if (data.success)
                {
                    data.msg = "删除成功";
                    data.response = blogArticle?.bID.ObjToString();
                }
            }

            return data;
        }
        /// <summary>
        /// apache jemeter 压力测试
        /// 更新接口
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("ApacheTestUpdate")]
        [AllowAnonymous]
        public async Task<MessageModel<bool>> ApacheTestUpdate()
        {
            return new MessageModel<bool>()
            {
                success = true,
                msg = "更新成功111",
                response = await _blogArticleServices.Update(new { bsubmitter = $"laozhang{DateTime.Now.Millisecond}", bID = 1 })
            };
        }
    }
}