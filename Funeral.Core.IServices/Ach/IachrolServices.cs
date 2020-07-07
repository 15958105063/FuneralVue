using Funeral.Core.IServices.BASE;
using Funeral.Core.Model.Models;
using System.Threading.Tasks;

namespace Funeral.Core.IServices
{	
	/// <summary>
	/// IAchRolServices
	/// </summary>	
    public interface IAchRolServices :IBaseServices<AchRol>
	{
		Task<string> SaveWordFile(string savePath, string tablename, int tid);
	}
}