using Funeral.Core.IServices.BASE;
using Funeral.Core.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Funeral.Core.IServices
{
    public partial interface IPermissionServices : IBaseServices<Permission>
    {
        Task<List<Permission>> QueryMuchTable();
    }
}