using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZID.Automat.Application;
using ZID.Automat.Application.Admin;
using ZID.Automat.Dto.Models.Analytics;
using ZID.Automat.Dto.Models.Analytics.User;

namespace ZID.Automat.Api.Controllers.Admin
{
    [Route("[controller]")]
    [ApiController]
    public class AUserInforController : ControllerBase
    {
        private readonly IAdminUserService _adminUserService;
        public AUserInforController(IAdminUserService adminUserService)
        {
            _adminUserService = adminUserService;
        }

        [HttpGet("User")]
        //[Authorize]
        public IEnumerable<UserAdminGetAll> GetAnalyticsItems()
        {
            var us = _adminUserService.GetAllUsers();
            return us;
        }
    }
}
