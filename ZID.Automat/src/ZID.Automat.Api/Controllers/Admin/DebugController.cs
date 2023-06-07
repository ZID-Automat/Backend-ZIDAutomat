using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZID.Automat.Application;

namespace ZID.Automat.Api.Controllers.Admin
{
    [ApiController]
    [Authorize(Roles = "Admin")]
    [Route("[controller]")]
    public class DebugController : ControllerBase
    {
        private readonly ISeedService SeedService;
        public DebugController(ISeedService seedService)
        {
            SeedService = seedService;
        }

        [HttpPost("Seed")]
        public IActionResult Seed()
        {
            SeedService.Seed();
            return Ok();
        }
    }
}
