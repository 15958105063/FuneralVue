using Funeral.Core.IRepository;
using Funeral.Core.IRepository.UnitOfWork;
using Funeral.Core.Model.Models;
using Funeral.Core.Repository.Base;

namespace Funeral.Core.Repository
{
	/// <summary>
	/// AchVdrRepository
	/// </summary>
    public class AchVdrRepository : BaseRepository<AchVdr>, IAchVdrRepository
    {
        public AchVdrRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}