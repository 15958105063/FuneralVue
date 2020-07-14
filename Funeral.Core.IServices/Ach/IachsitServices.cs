using Funeral.Core.IServices.BASE;
using Funeral.Core.Model.Models;
using System.Threading.Tasks;

namespace Funeral.Core.IServices
{	
	/// <summary>
	/// IAchSitServices
	/// </summary>	
    public interface IAchSitServices :IBaseServices<AchSit>
	{
		Task<string> SaveWordFile(string savePath, string tablename, int tid);
	}
}