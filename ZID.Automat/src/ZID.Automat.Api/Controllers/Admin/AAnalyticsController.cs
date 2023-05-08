using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZID.Automat.Application;
using ZID.Automat.Dto.Models;

namespace ZID.Automat.Api.Controllers.Admin
{
    [Route("[controller]")]
    [ApiController]
    public class AAnalyticsController : ControllerBase
    {
        private readonly AnalyticsService _analyticsService;
        public AAnalyticsController(AnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
        }

        [HttpGet("GetAnalyticsItems")]
        //[Authorize]
        public IActionResult GetAnalyticsItems()
        {
            IEnumerable<AnalyticItemDto> items = _analyticsService.GetAnalyticsItems();
            return Ok(items);
        }
    }
}
