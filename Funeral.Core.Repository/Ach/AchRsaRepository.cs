using Funeral.Core.IRepository;
using Funeral.Core.IRepository.UnitOfWork;
using Funeral.Core.Model.Models;
using Funeral.Core.Repository.Base;

namespace Funeral.Core.Repository
{
    /// <summary>
    /// AchRsaRepository
    /// </summary>
    public class AchRsaRepository : BaseRepository<AchRsa>, IAchRsaRepository
    {
        public AchRsaRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

    }
}