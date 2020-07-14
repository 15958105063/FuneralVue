
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
    /// 角色按钮关联配置
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]

    public class AchRacController : ControllerBase
    {
        private readonly INpoiWordExportServices _npoiWordExportServices;
        private readonly IAchRacServices _achRacServices;
        private readonly IMapper _mapper;
        readonly IUser _user;
        public AchRacController(INpoiWordExportServices npoiWordExportServices,IUser user, IMapper mapper,IAchRacServices achRacServices) {
            this._npoiWordExportServices = npoiWordExportServices;
            this._achRacServices = achRacServices;
            this._mapper = mapper;
            this._user = user;
        }



    }
}
