using Funeral.Core.IRepository;
using Funeral.Core.IRepository.UnitOfWork;
using Funeral.Core.Model.Models;
using Funeral.Core.Repository.Base;

namespace Funeral.Core.Repository
{
	/// <summary>
	/// AchSitRepository
	/// </summary>
    public class AchSitRepository : BaseRepository<AchSit>, IAchSitRepository
    {
        public AchSitRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}