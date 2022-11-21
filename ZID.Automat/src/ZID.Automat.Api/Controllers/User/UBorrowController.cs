using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;
using System.Security.Principal;
using ZID.Automat.Application;
using ZID.Automat.Domain.Models;
using ZID.Automat.Dto.Models;
using ZID.Automat.Repository;

namespace ZID.Automat.Api.Controllers.User
{
    [ApiController]
    [Authorize(Roles = "User")]
    [Route("[controller]")]
    public class UBorrowController
    {
        public readonly IBorrowService _borrowService;
        public UBorrowController(IBorrowService borrowService)
        {
            _borrowService = borrowService;
        }

        [HttpPost("Borrow")]
        public void Borrow(BorrowDataDto borrowData)
        {
            var username = ClaimsPrincipal.Current?.Claims.First(c => c.Type == "Name")??throw new Exception("can't red UserName");
            this._borrowService.Borrow(borrowData, username.Value, DateTime.Now);
        }
    }
}
