using Funeral.Core.IServices;
using Funeral.Core.IRepository;
using Funeral.Core.Services.BASE;
using Funeral.Core.Model.Models;
using System.Threading.Tasks;
using System.Linq;
using Funeral.Core.Common;
using Funeral.Core.IRepository.UnitOfWork;
using System.Collections.Generic;
using Funeral.Core.Common.HttpContextUser;

namespace Funeral.Core.Services
{	
	/// <summary>
	/// RoleServices
	/// </summary>	
	public class LoginInfoServices
    {

        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IRoleTenanRepository _roleTenanRepository;
        private readonly ITenanRepository _tenanRepository;
        readonly IUser _user;
        private readonly IUnitOfWork _unitOfWork;
        public LoginInfoServices(IUnitOfWork unitOfWork, IUserRoleRepository userRoleRepository, IRoleTenanRepository roleTenanRepository, ITenanRepository tenanRepository, IUser user)
        {
            this._unitOfWork = unitOfWork;
            this._userRoleRepository = userRoleRepository;
            this._roleTenanRepository = roleTenanRepository;
            this._tenanRepository = tenanRepository;
            this._user = user;
        }

        public async Task<Tenan> GetLoginTenan()
        {
            int roleid = ((await _userRoleRepository.Query(a => a.UserId == _user.ID)).OrderByDescending(a => a.Id).LastOrDefault()?.RoleId).ObjToInt();
            int tenanid = ((await _roleTenanRepository.Query(a => a.RoleId == roleid)).OrderByDescending(a => a.TenanId).LastOrDefault()?.TenanId).ObjToInt();

            return (await _tenanRepository.Query(a => a.Id == tenanid)).SingleOrDefault();
        }
    }
}
