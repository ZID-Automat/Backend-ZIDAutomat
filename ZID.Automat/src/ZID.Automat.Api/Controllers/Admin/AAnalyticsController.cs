using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using ZID.Automat.Application;
using ZID.Automat.Dto.Models.Analytics;
using ZID.Automat.Dto.Models.Analytics.User;

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
        public IEnumerable<AnalyticItemDto> GetAnalyticsItems()
        {
            return _analyticsService.GetAnalyticsItems();
        }

        [HttpGet("GesamtBorrows")]
        public IEnumerable<GesammtBorrowsDto> GesammtBorrow()
        {
            return _analyticsService.GesammtBorrows();
        }

        [HttpGet("WievielZuspat")]
        public IEnumerable<WieVielZuspaetDto> WievielZuspat()
        {
            return _analyticsService.WievielZuspat();
        }

        [HttpGet("TaeglicheUser")]
        public IEnumerable<TaeglicheUserDto> TaeglicheUser()
        {
            return _analyticsService.TaeglicheUser();
        }
    }
}
