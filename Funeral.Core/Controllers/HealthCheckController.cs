using Microsoft.AspNetCore.Mvc;

namespace Funeral.Core.Controllers
{
    /// <summary>
    /// 健康检查
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class HealthCheckController : ControllerBase
    {
        /// <summary>
        /// 健康检查接口
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}