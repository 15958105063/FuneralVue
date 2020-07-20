using Funeral.Core.IRepository;
using Funeral.Core.Repository.Base;
using Funeral.Core.Model.Models;
using Funeral.Core.IRepository.UnitOfWork;

namespace Funeral.Core.Repository
{
    /// <summary>
    /// PermissionTenanRepository
    /// </summary>	
    public class PermissionTenanRepository : BaseRepository<PermissionTenan>, IPermissionTenanRepository
    {
        public PermissionTenanRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
