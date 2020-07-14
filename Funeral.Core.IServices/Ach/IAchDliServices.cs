using Funeral.Core.IServices.BASE;
using Funeral.Core.Model.Models;
using System.Threading.Tasks;

namespace Funeral.Core.IServices
{
	/// <summary>
	/// IAchDliServices
	/// </summary>	
	public interface IAchDliServices :IBaseServices<AchDli>
	{
		Task<string> SaveWordFile(string savePath, string tablename, int tid);
	}
}