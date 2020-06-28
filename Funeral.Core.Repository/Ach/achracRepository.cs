using Funeral.Core.IRepository;
using Funeral.Core.IRepository.UnitOfWork;
using Funeral.Core.Model.Models;
using Funeral.Core.Repository.Base;

namespace Funeral.Core.Repository
{
	/// <summary>
	/// AchRacRepository
	/// </summary>
    public class AchRacRepository : BaseRepository<AchRac>, IAchRacRepository
    {
        public AchRacRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}