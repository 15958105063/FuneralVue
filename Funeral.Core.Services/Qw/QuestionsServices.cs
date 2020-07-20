using AutoMapper;
using Funeral.Core.Common.HttpContextUser;
using Funeral.Core.IRepository;
using Funeral.Core.IServices;
using Funeral.Core.Model;
using Funeral.Core.Model.Models;
using Funeral.Core.Model.ViewModels;
using Funeral.Core.Services.BASE;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Funeral.Core.Services
{
    public partial class QuestionsServices : BaseServices<Questions>, IQuestionsServices
    {
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IRoleTenanRepository _roleTenanRepository;
        private readonly ITenanRepository _tenanRepository;
        private readonly IQuestionsRepository _dal;
        readonly IUser _user;
        readonly IMapper _mapper;
        public QuestionsServices(IUserRoleRepository userRoleRepository, IRoleTenanRepository roleTenanRepository, ITenanRepository tenanRepository, IMapper mapper, IUser user, IQuestionsRepository dal)
        {
            this._userRoleRepository = userRoleRepository;
            this._roleTenanRepository = roleTenanRepository;
            this._tenanRepository = tenanRepository;
            this._mapper = mapper;
            this._user = user;
            this._dal = dal;
            base.BaseDal = dal;
        }

        /// <summary>
        /// 分页查询列表
        /// </summary>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <param name="orderby"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<PageModel<QuestionsDto>> GetListByPage(int pageindex = 1, int pagesize = 50, string orderby = "", string key = "")
        {
            Expression<Func<Questions, bool>> whereExpression = a => (a.Tid == GetLoginTenan().Id&&(a.Title.Contains(key)||a.Content.Contains(key)));
            var result= _mapper.Map<PageModel<QuestionsDto>>((await base.QueryPage(whereExpression, pageindex, pagesize, orderby)));
            return result;
        }

        /// <summary>
        /// 获取登录客户信息----可以考虑做到redis中去，登录时存入，退出时清除，时效和jwt时间一致，如果没有，则需要重新登录
        /// </summary>
        /// <returns></returns>
        public async Task<Tenan> GetLoginTenan()
        {
            int roleid = ((await _userRoleRepository.Query(a => a.UserId == _user.ID)).OrderByDescending(a => a.Id).LastOrDefault()?.RoleId).ObjToInt();
            int tenanid = ((await _roleTenanRepository.Query(a => a.RoleId == roleid)).OrderByDescending(a => a.TenanId).LastOrDefault()?.TenanId).ObjToInt();
       
            return (await _tenanRepository.Query(a => a.Id == tenanid)).SingleOrDefault();
        }

        public async Task<bool> PostQuestionInfo(QuestionsDto modeldto)
        {
            if (modeldto.Id > 0)
            {
                //更新
                Questions model = null;
                model = _mapper.Map<Questions>(modeldto);
                model.ModifyId = _user.ID;
                model.ModifyBy = _user.Name;
                model.ModifyTime = DateTime.Now;
               return await base.Update(model);

            }
            else
            {
                //新增
                Questions model = null;
                model = _mapper.Map<Questions>(modeldto);
                model.CreateId = _user.ID;
                model.CreateBy = _user.Name;
                model.CreateTime = DateTime.Now;
                model.Tid = GetLoginTenan().Id;
                return (await base.Add(model))>0?true:false;
            }
        }
    }
}