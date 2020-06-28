using Funeral.Core.IRepository;
using Funeral.Core.IRepository.UnitOfWork;
using Funeral.Core.Model.Models;
using Funeral.Core.Repository.Base;

namespace Funeral.Core.Repository
{
	/// <summary>
	/// AchAcsRepository
	/// </summary>
    public class AchAcsRepository : BaseRepository<AchAcs>, IAchAcsRepository
    {
        public AchAcsRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}