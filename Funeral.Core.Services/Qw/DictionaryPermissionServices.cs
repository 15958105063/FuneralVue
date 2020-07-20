using Funeral.Core.IRepository;
using Funeral.Core.IServices;
using Funeral.Core.Model.Models;
using Funeral.Core.Services.BASE;

namespace Funeral.Core.Services
{
    public partial class DictionaryPermissionServices : BaseServices<DictionaryPermission>, IDictionaryPermissionServices
    {
        private readonly IDictionaryPermissionRepository _dal;
        public DictionaryPermissionServices(IDictionaryPermissionRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
    }
}