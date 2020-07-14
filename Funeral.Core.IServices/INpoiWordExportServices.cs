

using System.Threading.Tasks;

namespace Funeral.Core.IServices
{
   public interface INpoiWordExportServices
    {
        bool SaveWordFileDefault (string savePath);
        Task<string> SaveWordFile(string savePath, string tablename1, string tablename2, string tablename3, string tablename4, int tid);
    }
}
