using Funeral.Core.Services.BASE;
using Funeral.Core.Model.Models;
using Funeral.Core.IRepository;
using Funeral.Core.IServices;

namespace Funeral.Core.Services
{	
	/// <summary>
	/// ModuleServices
	/// </summary>	
	public class ModuleServices : BaseServices<Modules>, IModuleServices
    {
	
        IModuleRepository _dal;
        public ModuleServices(IModuleRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
       
    }
}
