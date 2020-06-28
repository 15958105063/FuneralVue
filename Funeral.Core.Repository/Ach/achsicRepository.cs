using Funeral.Core.IRepository;
using Funeral.Core.IRepository.UnitOfWork;
using Funeral.Core.Model.Models;
using Funeral.Core.Repository.Base;

namespace Funeral.Core.Repository
{
	/// <summary>
	/// AchSicRepository
	/// </summary>
    public class AchSicRepository : BaseRepository<AchSic>, IAchSicRepository
    {
        public AchSicRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}