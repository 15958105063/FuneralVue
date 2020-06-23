using Funeral.Core.Repository.Base;
using Funeral.Core.Model.Models;
using Funeral.Core.IRepository;
using Funeral.Core.IRepository.UnitOfWork;

namespace Funeral.Core.Repository
{
    /// <summary>
    /// ModuleRepository
    /// </summary>	
    public class ModuleRepository : BaseRepository<Modules>, IModuleRepository
    {
        public ModuleRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
