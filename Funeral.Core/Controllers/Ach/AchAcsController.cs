
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
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AchAcsController : ControllerBase
    {
        private readonly INpoiWordExportServices _npoiWordExportServices;
        private readonly IAchAcsServices _achAcsServices;
        private readonly IMapper _mapper;
        readonly IUser _user;
        public AchAcsController(INpoiWordExportServices npoiWordExportServices,IUser user, IMapper mapper,IAchAcsServices achAcsServices) {
            this._npoiWordExportServices = npoiWordExportServices;
            this._achAcsServices = achAcsServices;
            this._mapper = mapper;
            this._user = user;
        }

    }
}
