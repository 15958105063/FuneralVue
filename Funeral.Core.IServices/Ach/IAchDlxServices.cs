using Funeral.Core.IServices.BASE;
using Funeral.Core.Model.Models;
using System.Threading.Tasks;

namespace Funeral.Core.IServices
{
	/// <summary>
	/// IAchDlxServices
	/// </summary>	
	public interface IAchDlxServices :IBaseServices<AchDlx>
	{
		Task<string> SaveWordFile(string savePath, string tablename, int tid);
	}
}