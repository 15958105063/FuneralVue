﻿

using System.Threading.Tasks;

namespace Funeral.Core.IServices
{
   public interface INpoiWordExportServices
    {
        bool SaveWordFileDefault (string savePath);
       Task<bool> SaveWordFile(string savePath,string tablename,int tid);
    }
}