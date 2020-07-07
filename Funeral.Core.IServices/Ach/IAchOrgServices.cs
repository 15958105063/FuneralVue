using Funeral.Core.IServices.BASE;
using Funeral.Core.Model.Models;
using System.Threading.Tasks;

namespace Funeral.Core.IServices
{
    /// <summary>
    /// IAchOrgServices
    /// </summary>
    public interface IAchOrgServices : IBaseServices<AchOrg>
    {
        Task<string> SaveWordFile(string savePath, string tablename, int tid);
    }
}
