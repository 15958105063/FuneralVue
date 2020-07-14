using Funeral.Core.IServices.BASE;
using Funeral.Core.Model.Models;
using System.Threading.Tasks;

namespace Funeral.Core.IServices
{
	/// <summary>
	/// IAchRsaServices
	/// </summary>	
	public interface IAchRsaServices :IBaseServices<AchRsa>
	{
		Task<string> SaveWordFile(string savePath, string tablename, int tid);
	}
}