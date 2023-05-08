using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZID.Automat.Application;

namespace ZID.Automat.Api.Controllers.Admin
{
    [Route("[controller]")]
    [ApiController]
    public class AAnalyticsController : ControllerBase
    {
        public AAnalyticsController()
        {
        }

        [HttpGet("AnalyticsItems")]
        //[Authorize]
        public IActionResult GetAnalyticsItems()
        {
            var items = AnalyticsService.GetAnalyticsItems();
            return Ok(items);
        }
    }
}
