using Funeral.Core.IServices.BASE;
using Funeral.Core.Model.Models;
using System.Threading.Tasks;

namespace Funeral.Core.IServices
{
	/// <summary>
	/// IAchCauServices
	/// </summary>	
	public interface IAchCauServices :IBaseServices<AchCau>
	{
		Task<string> SaveWordFile(string savePath, string tablename, int tid);
	}
}