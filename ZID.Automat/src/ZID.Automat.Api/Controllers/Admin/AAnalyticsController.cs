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
        private readonly IAnalyticsService _analyticsService;
        public AAnalyticsController(IAnalyticsService analyticsService)
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
