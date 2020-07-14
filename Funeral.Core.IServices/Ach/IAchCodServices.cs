using Funeral.Core.IServices.BASE;
using Funeral.Core.Model.Models;
using System.Threading.Tasks;

namespace Funeral.Core.IServices
{
	/// <summary>
	/// IAchCodServices
	/// </summary>	
	public interface IAchCodServices :IBaseServices<AchCod>
	{
		Task<string> SaveWordFile(string savePath, string tablename, int tid);
	}
}