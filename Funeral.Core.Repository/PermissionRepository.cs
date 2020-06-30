using Funeral.Core.IRepository;
using Funeral.Core.IRepository.UnitOfWork;
using Funeral.Core.Model.Models;
using Funeral.Core.Repository.Base;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Funeral.Core.Repository
{
    public class PermissionRepository : BaseRepository<Permission>, IPermissionRepository
    {
        public PermissionRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        public async Task<List<Permission>> QueryMuchTable()
        {
            return await QueryMuch<Permission, Modules, Permission>(
                (rmp, p) => new object[] {
                    JoinType.Left, rmp.Mid == p.Id,
                },

                (rmp, p) => new Permission()
                {
                    Id = rmp.Id,
                    Name = rmp.Name,
                    Pid = rmp.Pid,
                    OrderSort = rmp.OrderSort,
                    Code = rmp.Code,
                    Icon = rmp.Icon,
                    Func = rmp.Func,
                    IsHide = rmp.IsHide,
                    IsButton = rmp.IsButton,
                    MName = p.Name,
                    Mid = rmp.Mid,
                    Enabled = rmp.Enabled,
                    Description = rmp.Description
                },
                (rmp, p) => rmp.IsDeleted == false && rmp.Enabled == true
                );
        }

    }
}
