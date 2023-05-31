using Microsoft.AspNetCore.Mvc;
using ZID.Automat.Application.Admin;
using ZID.Automat.Dto.Models.Analytics.User;

namespace ZID.Automat.Api.Controllers.Admin
{
    public class ABorrowInfoController
    {
        private readonly IAdminBorrowService _adminBorrowService;
        public ABorrowInfoController(IAdminBorrowService adminBorrowService)
        {
            _adminBorrowService = adminBorrowService;
        }


        [HttpGet("BorrowDetailed")]
        public BorrowAdminDetailedDto BorrowDetailed(int id)
        {
           return _adminBorrowService.BorrowDetailed(id);
        }
    }
}
