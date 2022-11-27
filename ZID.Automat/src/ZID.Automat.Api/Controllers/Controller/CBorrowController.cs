using Microsoft.AspNetCore.Mvc;
using ZID.Automat.Application;
using ZID.Automat.Dto.Models;
using ZID.Automat.Repository;

namespace ZID.Automat.Api.Controllers.Controller
{
    public class CBorrowController:ControllerBase
    {
        public readonly IItemService _itemService;
        public readonly IQrCodeCService _qrCodeService;

        public CBorrowController(IItemService itemService, IQrCodeCService qrCodeService)
        {
            _itemService = itemService;
            _qrCodeService = qrCodeService;
        }
        
        [HttpGet("ValidateQrCode")]
        public ValidQrCodeDto ValidateQrCode(QrCodeDto qrCodeDto)
        {
            return new ValidQrCodeDto() { valid = _qrCodeService.IsValidQrCode(qrCodeDto) };
        }

        [HttpGet("LoadItemData")]
        public ItemDetailedDto LoadItemData(QrCodeDto qrCodeDto)
        {
            return _itemService.DetailedItem(qrCodeDto.QRCode);
        }
        
        [HttpPut("InvalidateQrCode")]
        public void InvalidateQrCode(QrCodeDto qrCodeDto)
        {
            _qrCodeService.InvalidateQrCode(qrCodeDto, DateTime.Now);
        }
    }
}
