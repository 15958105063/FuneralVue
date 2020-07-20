using Funeral.Core.IRepository;
using Funeral.Core.IServices;
using Funeral.Core.Model.Models;
using Funeral.Core.Services.BASE;

namespace Funeral.Core.Services
{
    public partial class DataDictionaryServices : BaseServices<DataDictionary>, IDataDictionaryServices
    {
        private readonly IDataDictionaryRepository _dal;
        public DataDictionaryServices(IDataDictionaryRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
    }
}