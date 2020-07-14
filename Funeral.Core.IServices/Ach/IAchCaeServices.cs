using Funeral.Core.IServices.BASE;
using Funeral.Core.Model.Models;
using System.Threading.Tasks;

namespace Funeral.Core.IServices
{
	/// <summary>
	/// IAchCaeServices
	/// </summary>	
	public interface IAchCaeServices :IBaseServices<AchCae>
	{
		Task<string> SaveWordFile(string savePath, string tablename, int tid);
	}
}