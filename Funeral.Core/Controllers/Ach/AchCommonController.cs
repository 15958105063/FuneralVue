
using System.Threading.Tasks;
using Funeral.Core.IServices;
using Funeral.Core.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Funeral.Core.Controllers.Ach
{

    [Route("api/[controller]/[action]")]
    [ApiController]
    
    public class AchCommonController : ControllerBase
    {
        private readonly INpoiWordExportServices  _NpoiWordExportServices;

        public AchCommonController(INpoiWordExportServices NpoiWordExportServices) {
            this._NpoiWordExportServices = NpoiWordExportServices;
        }

        [HttpGet]
        [Route("Export")]
        [AllowAnonymous]
        public async Task<MessageModel<string>> Export(string savePath, string tablename, int tid) {
        bool result=  await  _NpoiWordExportServices.SaveWordFile(savePath, tablename, tid);

            return new MessageModel<string>()
            {
                msg = "导出成功",
                success = result
            };
        }
    }
}
