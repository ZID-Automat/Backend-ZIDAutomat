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


        [HttpPost("Entschuldigt")]
        public void Entschuldigt(ValIdObjectDto ob )
        {
             _adminBorrowService.Entschuldigt(ob.Id, ob.Value);
        }


        [HttpPost("Zurueckgeben")]
        public DateTime? Zurueckgeben(ValIdObjectDto ob)
        {
            return _adminBorrowService.Zurueckgeben(ob.Id,ob.Value);
        }
    }
}
