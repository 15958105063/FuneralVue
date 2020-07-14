
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

    }
}
