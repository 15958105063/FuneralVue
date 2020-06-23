using Funeral.Core.IRepository;
using Funeral.Core.Repository.Base;
using Funeral.Core.Model.Models;
using Funeral.Core.IRepository.UnitOfWork;

namespace Funeral.Core.Repository
{
    /// <summary>
    /// TenanRepository
    /// </summary>	
    public class TenanRepository : BaseRepository<Tenan>, ITenanRepository
    {
        public TenanRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
