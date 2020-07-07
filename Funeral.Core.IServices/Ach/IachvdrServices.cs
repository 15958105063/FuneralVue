using Funeral.Core.IServices.BASE;
using Funeral.Core.Model.Models;
using System.Threading.Tasks;

namespace Funeral.Core.IServices
{	
	/// <summary>
	/// IAchVdrServices
	/// </summary>	
    public interface IAchVdrServices :IBaseServices<AchVdr>
	{
		Task<string> SaveWordFile(string savePath, string tablename, int tid);
	}
}