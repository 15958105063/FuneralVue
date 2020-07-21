using AutoMapper;
using Funeral.Core.Common.HttpContextUser;
using Funeral.Core.IRepository;
using Funeral.Core.IServices;
using Funeral.Core.Model.Models;
using Funeral.Core.Model.ViewModels;
using Funeral.Core.Services.BASE;
using System;
using System.Threading.Tasks;

namespace Funeral.Core.Services
{
    public partial class AnswersServices : BaseServices<Answers>, IAnswersServices
    {
        private readonly IAnswersRepository _dal;
        readonly IMapper _mapper;
        readonly IUser _user;
        public AnswersServices(IAnswersRepository dal, IMapper mapper, IUser user)
        {
            this._mapper = mapper;
            this._user = user;
            this._dal = dal;
            base.BaseDal = dal;
        }


        public async Task<bool> PostAnswersInfo(AnswersDto modeldto)
        {
            if (modeldto.Id > 0)
            {
                //更新
                Answers model = null;
                model = _mapper.Map<Answers>(modeldto);
                model.ModifyId = _user.ID;
                model.ModifyBy = _user.Name;
                model.ModifyTime = DateTime.Now;
                return await base.Update(model);

            }
            else
            {
                //新增
                Answers model = null;
                model = _mapper.Map<Answers>(modeldto);
                model.CreateId = _user.ID;
                model.CreateBy = _user.Name;
                model.CreateTime = DateTime.Now;
               //model.Tid = GetLoginTenan().Id;
                return (await base.Add(model)) > 0 ? true : false;
            }
        }

    }
}