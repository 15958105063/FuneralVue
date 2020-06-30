using Funeral.Core.IRepository;
using Funeral.Core.Repository.Base;
using Funeral.Core.Model.Models;
using Funeral.Core.IRepository.UnitOfWork;
using System.Threading.Tasks;
using System.Collections.Generic;
using SqlSugar;
using Funeral.Core.Model;
using System.Linq.Expressions;
using System;

namespace Funeral.Core.Repository
{
    /// <summary>
    /// RoleRepository
    /// </summary>	
    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        public RoleRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<List<Role>> QueryMuchTable(int tid)
        {
            return await QueryMuch<RoleTenan, Role, Role>(
                (rmp, m) => new object[] {
                    JoinType.Left, rmp.TenanId == m.Id,
                },

                (rmp, m) => new Role()
                {
                    Id = m.Id,
                    Name = m.Name
                },

                (rmp, m) => m.Enabled == true && rmp.TenanId == tid
                );
        }


    }
}
