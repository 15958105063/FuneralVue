using Funeral.Core.IServices.BASE;
using Funeral.Core.Model.Models;
using System.Threading.Tasks;

namespace Funeral.Core.IServices
{	
	/// <summary>
	/// IAchUsrServices
	/// </summary>	
    public interface IAchUsrServices :IBaseServices<AchUsr>
	{
		Task<string> SaveWordFile(string savePath, string tablename, int tid);
		Task<string> SaveWordFile(string savePath, string tablename, string linktablename,int tid);
	}
}