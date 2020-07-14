using Funeral.Core.IServices.BASE;
using Funeral.Core.Model.Models;
using System.Threading.Tasks;

namespace Funeral.Core.IServices
{
	/// <summary>
	/// IAchCtpServices
	/// </summary>	
	public interface IAchCtpServices :IBaseServices<AchCtp>
	{
		Task<string> SaveWordFile(string savePath, string tablename, int tid);
	}
}