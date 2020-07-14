using Funeral.Core.IRepository;
using Funeral.Core.IRepository.UnitOfWork;
using Funeral.Core.Model.Models;
using Funeral.Core.Repository.Base;

namespace Funeral.Core.Repository
{
    /// <summary>
    /// AchCodRepository
    /// </summary>
    public class AchCodRepository : BaseRepository<AchCod>, IAchCodRepository
    {
        public AchCodRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

    }
}