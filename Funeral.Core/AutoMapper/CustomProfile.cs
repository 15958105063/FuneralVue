using AutoMapper;
using Funeral.Core.Model;
using Funeral.Core.Model.Models;
using Funeral.Core.Model.ViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Funeral.Core.AutoMapper
{
    public class CustomProfile : Profile
    {
        /// <summary>
        /// 配置构造函数，用来创建关系映射
        /// </summary>
        public CustomProfile()
        {
            #region BlogArticle
            CreateMap<BlogArticle, BlogViewModels>();
            CreateMap<BlogViewModels, BlogArticle>();
            #endregion

            #region AchOrg
            CreateMap<AchOrg, AchOrgInputViewModels>();
            CreateMap<AchOrgInputViewModels, AchOrg>();

            CreateMap(typeof(PageModel<AchOrg>), typeof(PageModel<AchOrgInputViewModels>));
            CreateMap(typeof(PageModel<AchOrgInputViewModels>), typeof(PageModel<AchOrg>));
            #endregion


            #region QW模块

            #region QuestionsDto
            CreateMap<Questions, QuestionsDto>();
            CreateMap<QuestionsDto, Questions>();

            CreateMap<PageModel<Questions>, PageModel<QuestionsDto>>();
            CreateMap<PageModel<QuestionsDto>, PageModel<Questions>>();

            #endregion

            #region AnswersDto
            CreateMap<Answers, AnswersDto>();
            CreateMap<AnswersDto, Answers>();

            CreateMap<PageModel<Answers>, PageModel<AnswersDto>>();
            CreateMap<PageModel<AnswersDto>, PageModel<Answers>>();

            #endregion


            #endregion

        }
    }
}
