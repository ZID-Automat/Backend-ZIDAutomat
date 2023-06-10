using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZID.Automat.Application;
using ZID.Automat.Application.Admin;
using ZID.Automat.Dto.Models.Analytics;
using ZID.Automat.Dto.Models.Analytics.User;

namespace ZID.Automat.Api.Controllers.Admin
{
    [ApiController]
    [Authorize(Roles = "Admin")]
    [Route("[controller]")]
    public class AdminLogShow : ControllerBase
    {
        private readonly IAdminLogShowSerivice _adminLogShowSerivice;
        public AdminLogShow(IAdminLogShowSerivice adminLogShowSerivice)
        {
            _adminLogShowSerivice = adminLogShowSerivice;
        }

        [HttpGet("InvalidItems")]
        public IEnumerable<LogQrCodeAdminDto> InvalidItems()
        {
           return _adminLogShowSerivice.InvalidItems();
        }


        [HttpGet("ScannedItems")]
        public IEnumerable<LogQrCodeAdminDto> ScannedItems()
        {
            return _adminLogShowSerivice.ScannedItems();
        }


        [HttpGet("EjectedItems")]
        public IEnumerable<LogQrCodeAdminDto> EjectedItems()
        {
            return _adminLogShowSerivice.EjectedItems();
        }
    }
}
