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
    [Authorize(Roles = "Admin")]
    public class AUserInforController : ControllerBase
    {
        private readonly IAdminUserService _adminUserService;
        public AUserInforController(IAdminUserService adminUserService)
        {
            _adminUserService = adminUserService;
        }

        [HttpGet("GetAllUsers")]
        public IEnumerable<UserAdminGetAll> GetAllUsers()
        {
            return _adminUserService.GetAllUsers();
        }

        [HttpGet("DetailedUser")]
        public UserAdminDetailedDto DetailedUser (int id)
        {
            return _adminUserService.GetDetailedUser(id);
        }

        [HttpPost("SetBlockiert")]
        public void SetBlockiert(BlockiertStateDto blockiertState)
        {
            _adminUserService.SetBlockiert(blockiertState.Id, blockiertState.Blockiert);
        }
    }
}
