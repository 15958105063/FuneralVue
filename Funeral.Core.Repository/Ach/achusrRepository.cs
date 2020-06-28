using Funeral.Core.IRepository;
using Funeral.Core.IRepository.UnitOfWork;
using Funeral.Core.Model.Models;
using Funeral.Core.Repository.Base;

namespace Funeral.Core.Repository
{
	/// <summary>
	/// AchUsrRepository
	/// </summary>
    public class AchUsrRepository : BaseRepository<AchUsr>, IAchUsrRepository
    {
        public AchUsrRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}