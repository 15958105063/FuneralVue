using Funeral.Core.IRepository;
using Funeral.Core.Repository.Base;
using Funeral.Core.Model.Models;
using Funeral.Core.IRepository.UnitOfWork;

namespace Funeral.Core.Repository
{
    /// <summary>
    /// AchOrgRepository
    /// </summary>	
    public class AchOrgRepository : BaseRepository<AchOrg>, IAchOrgRepository
    {
        public AchOrgRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
