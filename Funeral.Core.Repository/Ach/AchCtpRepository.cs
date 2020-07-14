using Funeral.Core.IRepository;
using Funeral.Core.IRepository.UnitOfWork;
using Funeral.Core.Model.Models;
using Funeral.Core.Repository.Base;

namespace Funeral.Core.Repository
{
    /// <summary>
    /// AchDlxRepository
    /// </summary>
    public class AchCtpRepository : BaseRepository<AchCtp>, IAchCtpRepository
    {
        public AchCtpRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

    }
}