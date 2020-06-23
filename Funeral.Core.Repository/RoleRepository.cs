using Funeral.Core.IRepository;
using Funeral.Core.Repository.Base;
using Funeral.Core.Model.Models;
using Funeral.Core.IRepository.UnitOfWork;

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
    }
}
