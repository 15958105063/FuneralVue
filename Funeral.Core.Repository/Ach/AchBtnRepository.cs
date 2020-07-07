using Funeral.Core.IRepository;
using Funeral.Core.IRepository.UnitOfWork;
using Funeral.Core.Model.Models;
using Funeral.Core.Repository.Base;

namespace Funeral.Core.Repository
{
    /// <summary>
    /// AchBtnRepository
    /// </summary>
    public class AchBtnRepository : BaseRepository<AchBtn>, IAchBtnRepository
    {
        public AchBtnRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}