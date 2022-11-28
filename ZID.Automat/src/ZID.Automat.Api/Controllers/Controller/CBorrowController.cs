using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZID.Automat.Application;
using ZID.Automat.Dto.Models;
using ZID.Automat.Repository;

namespace ZID.Automat.Api.Controllers.Controller
{
    [ApiController]
    [Authorize(Roles = "Controller")]
    [Route("[controller]")]
    public class CBorrowController:ControllerBase
    {
        public readonly IItemService _itemService;
        public readonly IQrCodeCService _qrCodeService;

        public CBorrowController(IItemService itemService, IQrCodeCService qrCodeService)
        {
            _itemService = itemService;
            _qrCodeService = qrCodeService;
        }
        
        [HttpPost("ValidateQrCode")]
        public ValidQrCodeDto ValidateQrCode(QrCodeDto qrCodeDto)
        {
            return _qrCodeService.IsValidQrCode(qrCodeDto);
        }

        [HttpGet("LoadItemData")]
        public ItemDetailedDto LoadItemData(int item)
        {
            return _itemService.DetailedItem(item);
        }
        
        [HttpPut("InvalidateQrCode")]
        public void InvalidateQrCode(QrCodeDto qrCodeDto)
        {
            _qrCodeService.InvalidateQrCode(qrCodeDto, DateTime.Now);
        }
    }
}
