using Funeral.Core.IRepository;
using Funeral.Core.IRepository.UnitOfWork;
using Funeral.Core.Model.Models;
using Funeral.Core.Repository.Base;

namespace Funeral.Core.Repository
{
    /// <summary>
    /// AchPvlRepository
    /// </summary>
    public class AchPvlRepository : BaseRepository<AchPvl>, IAchPvlRepository
    {
        public AchPvlRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

    }
}