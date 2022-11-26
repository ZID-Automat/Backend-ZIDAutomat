using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;
using System.Security.Principal;
using ZID.Automat.Application;
using ZID.Automat.Dto.Models;
using ZID.Automat.Repository;

namespace ZID.Automat.Api.Controllers.User
{
    [ApiController]
    [Authorize(Roles = "User")]
    [Route("[controller]")]
    public class UBorrowController: Controller
    {
        public readonly IBorrowService _borrowService;
        public UBorrowController(IBorrowService borrowService)
        {
            _borrowService = borrowService;
        }

        [HttpPost("Borrow")]
        public BorrowResponseDto Borrow(BorrowDataDto borrowData)
        {
            var username = User.Claims.First(c => c.Type == "Name");    
            string qrCode = _borrowService.Borrow(borrowData, username.Value, DateTime.Now);
            return new BorrowResponseDto() { QRCode = qrCode };
        }
    }
}
