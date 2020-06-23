

using System.Threading.Tasks;

namespace Funeral.Core.IServices
{
   public interface INpoiWordExportService
    {
        bool SaveWordFileDefault (string savePath);
       Task<bool> SaveWordFile(string savePath);
    }
}
