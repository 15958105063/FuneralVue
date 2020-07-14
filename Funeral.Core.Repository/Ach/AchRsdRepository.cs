using Funeral.Core.IRepository;
using Funeral.Core.IRepository.UnitOfWork;
using Funeral.Core.Model.Models;
using Funeral.Core.Repository.Base;

namespace Funeral.Core.Repository
{
    /// <summary>
    /// AchRsdRepository
    /// </summary>
    public class AchRsdRepository : BaseRepository<AchRsd>, IAchRsdRepository
    {
        public AchRsdRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

    }
}