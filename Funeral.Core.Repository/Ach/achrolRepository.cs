using Funeral.Core.IRepository;
using Funeral.Core.IRepository.UnitOfWork;
using Funeral.Core.Model.Models;
using Funeral.Core.Repository.Base;

namespace Funeral.Core.Repository
{
	/// <summary>
	/// AchRolRepository
	/// </summary>
    public class AchRolRepository : BaseRepository<AchRol>, IAchRolRepository
    {
        public AchRolRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}