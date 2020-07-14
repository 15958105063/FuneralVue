using Funeral.Core.IServices.BASE;
using Funeral.Core.Model.Models;
using System.Threading.Tasks;

namespace Funeral.Core.IServices
{
	/// <summary>
	/// IAchRsdServices
	/// </summary>	
	public interface IAchRsdServices :IBaseServices<AchRsd>
	{
		Task<string> SaveWordFile(string savePath, string tablename, int tid);
	}
}