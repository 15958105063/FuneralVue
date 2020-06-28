using Funeral.Core.IRepository;
using Funeral.Core.IRepository.UnitOfWork;
using Funeral.Core.Model.Models;
using Funeral.Core.Repository.Base;

namespace Funeral.Core.Repository
{
	/// <summary>
	/// AchDptRepository
	/// </summary>
    public class AchDptRepository : BaseRepository<AchDpt>, IAchDptRepository
    {
        public AchDptRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}