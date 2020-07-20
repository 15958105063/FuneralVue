using Funeral.Core.IRepository;
using Funeral.Core.IRepository.UnitOfWork;
using Funeral.Core.Model.Models;
using Funeral.Core.Repository.Base;

namespace Funeral.Core.Repository
{
    /// <summary>
    /// DataDictionaryRepository
    /// </summary>
    public class DataDictionaryRepository : BaseRepository<DataDictionary>, IDataDictionaryRepository
    {
        public DataDictionaryRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}