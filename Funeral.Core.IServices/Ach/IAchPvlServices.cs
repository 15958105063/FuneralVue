using Funeral.Core.IServices.BASE;
using Funeral.Core.Model.Models;
using System.Threading.Tasks;

namespace Funeral.Core.IServices
{
	/// <summary>
	/// IAchPvlServices
	/// </summary>	
	public interface IAchPvlServices :IBaseServices<AchPvl>
	{
		Task<string> SaveWordFile(string savePath, string tablename, int tid);
	}
}