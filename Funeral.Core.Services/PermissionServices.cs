using Funeral.Core.Services.BASE;
using Funeral.Core.Model.Models;
using Funeral.Core.IRepository;
using Funeral.Core.IServices;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System;

namespace Funeral.Core.Services
{	
	/// <summary>
	/// PermissionServices
	/// </summary>	
	public class PermissionServices : BaseServices<Permission>, IPermissionServices
    {
	
        IPermissionRepository _dal;
        public PermissionServices(IPermissionRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }

        public async Task<List<Permission>> QueryMuchTable()
        {
            return await _dal.QueryMuchTable();
        }

    }
}
