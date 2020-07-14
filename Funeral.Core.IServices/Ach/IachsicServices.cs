using Funeral.Core.IServices.BASE;
using Funeral.Core.Model.Models;
using System.Threading.Tasks;

namespace Funeral.Core.IServices
{	
	/// <summary>
	/// IAchSicServices
	/// </summary>	
    public interface IAchSicServices :IBaseServices<AchSic>
	{
		Task<string> SaveWordFile(string savePath, string tablename, int tid);
	}
}