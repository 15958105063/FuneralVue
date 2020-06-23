using Funeral.Core.IServices.BASE;
using Funeral.Core.Model.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Funeral.Core.IServices
{
    /// <summary>
    /// ITenanServices
    /// </summary>	
    public interface ITenanServices : IBaseServices<Tenan>
	{
        Task<Tenan> SaveTenan(string tenanName);
        Task<string> GetTenanNameByTid(int tid);

        Task<List<Tenan>> GetAll();
    }
}
