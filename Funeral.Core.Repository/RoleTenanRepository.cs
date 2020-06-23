using Funeral.Core.FrameWork.IRepository;
using Funeral.Core.Repository.Base;
using Funeral.Core.Model.Models;
using Funeral.Core.IRepository.UnitOfWork;

namespace Funeral.Core.Repository
{
    /// <summary>
    /// RoleTenanRepository
    /// </summary>	
    public class RoleTenanRepository : BaseRepository<RoleTenan>, IRoleTenanRepository
    {
        public RoleTenanRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
