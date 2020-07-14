using Funeral.Core.IServices.BASE;
using Funeral.Core.Model.Models;
using System.Threading.Tasks;

namespace Funeral.Core.IServices
{
	/// <summary>
	/// IAchRscServices
	/// </summary>	
	public interface IAchRscServices :IBaseServices<AchRsc>
	{
		Task<string> SaveWordFile(string savePath, string tablename, int tid);
	}
}