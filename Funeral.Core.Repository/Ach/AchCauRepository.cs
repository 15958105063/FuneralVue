using Funeral.Core.IRepository;
using Funeral.Core.IRepository.UnitOfWork;
using Funeral.Core.Model.Models;
using Funeral.Core.Repository.Base;

namespace Funeral.Core.Repository
{
    /// <summary>
    /// AchCauRepository
    /// </summary>
    public class AchCauRepository : BaseRepository<AchCau>, IAchCauRepository
    {
        public AchCauRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

    }
}