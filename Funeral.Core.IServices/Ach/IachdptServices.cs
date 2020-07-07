using Funeral.Core.IServices.BASE;
using Funeral.Core.Model.Models;
using System.Threading.Tasks;

namespace Funeral.Core.IServices
{	
	/// <summary>
	/// IAchDptServices
	/// </summary>	
    public interface IAchDptServices :IBaseServices<AchDpt>
	{
		Task<string> SaveWordFile(string savePath, string tablename, int tid);
	}
}