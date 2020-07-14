using Funeral.Core.IRepository;
using Funeral.Core.IRepository.UnitOfWork;
using Funeral.Core.Model.Models;
using Funeral.Core.Repository.Base;

namespace Funeral.Core.Repository
{
    /// <summary>
    /// AchRscRepository
    /// </summary>
    public class AchRscRepository : BaseRepository<AchRsc>, IAchRscRepository
    {
        public AchRscRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

    }
}