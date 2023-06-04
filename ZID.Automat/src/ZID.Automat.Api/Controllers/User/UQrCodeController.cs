using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZID.Automat.Application;
using ZID.Automat.Dto.Models;

namespace ZID.Automat.Api.Controllers.User
{
    [ApiController]
    [Authorize(Roles = "User")]
    [Route("[controller]")]
    public class UQrCodeController:ControllerBase
    {
        private readonly IQrCodeUService _qrCodeUService;
        public UQrCodeController(IQrCodeUService qrCodeUService)
        {
            _qrCodeUService = qrCodeUService;
        }

        [HttpGet("ActiveQrCodes")]
        public IEnumerable<BorrowDto> getActiveQrCodes()
        {
            var username = User.Claims.First(c => c.Type == "Name");
            return _qrCodeUService.OpenQrCodes(username.Value);
        }

        [HttpGet("AllQrCodes")]
        public IEnumerable<BorrowDto> getAllQrCodes()
        {
            var username = User.Claims.First(c => c.Type == "Name");
            return _qrCodeUService.AllQrCodes(username.Value);
        }

        [HttpGet("ActiveQrCodesCount")]
        public int getActiveQrCodesCount()
        {
            var username = User.Claims.First(c => c.Type == "Name");
            return _qrCodeUService.OpenQrCodesCount(username.Value);
        }
    }
}
