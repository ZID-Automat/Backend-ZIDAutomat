using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZID.Automat.Application;
using ZID.Automat.Dto.Models;

namespace ZID.Automat.Api.Controllers.User
{
    [ApiController]
    [Authorize(Roles = "User")]
    [Route("[controller]")]
    public class UQrCodeController
    {
        private readonly IQrCodeUService _qrCodeUService;
        public UQrCodeController(IQrCodeUService qrCodeUService)
        {
            _qrCodeUService = qrCodeUService;
        }

        [HttpGet("ActiveQrCodes")]
        public IEnumerable<BorrowDto> getActiveQrCodes()
        {
            return _qrCodeUService.OpenQrCodes();
        }
    }
}
