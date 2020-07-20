using Funeral.Core.IRepository;
using Funeral.Core.IRepository.UnitOfWork;
using Funeral.Core.Model.Models;
using Funeral.Core.Repository.Base;

namespace Funeral.Core.Repository
{
    /// <summary>
    /// DictionaryPermissionRepository
    /// </summary>
    public class DictionaryPermissionRepository : BaseRepository<DictionaryPermission>, IDictionaryPermissionRepository
    {
        public DictionaryPermissionRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}