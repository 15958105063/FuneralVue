using Funeral.Core.IRepository;
using Funeral.Core.IRepository.UnitOfWork;
using Funeral.Core.Model.Models;
using Funeral.Core.Repository.Base;

namespace Funeral.Core.Repository
{
    /// <summary>
    /// AchFgpRepository
    /// </summary>
    public class AchFunRepository : BaseRepository<AchFun>, IAchFunRepository
    {
        public AchFunRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}