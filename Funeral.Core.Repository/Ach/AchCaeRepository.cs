using Funeral.Core.IRepository;
using Funeral.Core.IRepository.UnitOfWork;
using Funeral.Core.Model.Models;
using Funeral.Core.Repository.Base;

namespace Funeral.Core.Repository
{
    /// <summary>
    /// AchCaeRepository
    /// </summary>
    public class AchCaeRepository : BaseRepository<AchCae>, IAchCaeRepository
    {
        public AchCaeRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

    }
}