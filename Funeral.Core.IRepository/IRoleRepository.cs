using Funeral.Core.IRepository.Base;
using Funeral.Core.Model.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Funeral.Core.IRepository
{	
	/// <summary>
	/// IRoleRepository
	/// </summary>	
	public interface IRoleRepository : IBaseRepository<Role>//类名
    {

		 Task<List<Role>> QueryMuchTable(int tid);

	}
}
