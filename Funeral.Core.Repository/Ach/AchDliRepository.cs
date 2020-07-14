using Funeral.Core.IRepository;
using Funeral.Core.IRepository.UnitOfWork;
using Funeral.Core.Model.Models;
using Funeral.Core.Repository.Base;

namespace Funeral.Core.Repository
{
    /// <summary>
    /// AchDliRepository
    /// </summary>
    public class AchDliRepository : BaseRepository<AchDli>, IAchDliRepository
    {
        public AchDliRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}