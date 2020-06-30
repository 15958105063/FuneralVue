
using Funeral.Core.IRepository.Base;
using Funeral.Core.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Funeral.Core.IRepository
{
    public partial interface IPermissionRepository : IBaseRepository<Permission>
    {

        Task<List<Permission>> QueryMuchTable();
    }
}