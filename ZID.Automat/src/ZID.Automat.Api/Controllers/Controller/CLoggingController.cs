using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZID.Automat.Application;
using ZID.Automat.Dto.Models;

namespace ZID.Automat.Api.Controllers.Controller
{
    [ApiController]
    [Route("[controller]")]
    public class CLoggingController: ControllerBase
    {
        private readonly IAutomatLoggingService _automatLogging;
        public CLoggingController(IAutomatLoggingService automatLogging)
        {
            _automatLogging = automatLogging;
        }

        [HttpPost("LogEjectedItem")]
        public void LogEjectedItem([FromBody] LogQrCodeDto qrCode)
        {
            _automatLogging.EjectedItem(qrCode.Guid);
        }

        [HttpPost("LogInvaldScannedQrCode")]
        public void LogInvaldScannedQrCode([FromBody] LogQrCodeDto qrCode)
        {
            _automatLogging.LogInvaldScannedQrCode(qrCode.Guid);
        }

        [HttpPost("LogScannedQrCode")]
        public void LogScannedQrCode([FromBody]LogQrCodeDto qrCode)
        {
            _automatLogging.LogScannedQrCode(qrCode.Guid);
        }
    }
}
