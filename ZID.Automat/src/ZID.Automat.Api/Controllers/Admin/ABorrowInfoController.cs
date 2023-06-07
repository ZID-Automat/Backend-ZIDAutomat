using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZID.Automat.Application.Admin;
using ZID.Automat.Dto.Models.Analytics.User;

namespace ZID.Automat.Api.Controllers.Admin
{
    [ApiController]
    [Authorize(Roles = "Admin")]
    [Route("[controller]")]
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



        [HttpGet("AllBorrows")]
        public IEnumerable<UserAdmiBorrowDto> AllBorrows()
        {
            return _adminBorrowService.AllBorrows();
        }

        [HttpGet("ToDealWithBorrows")]
        public IEnumerable<UserAdmiBorrowDto> ToDealWithBorrows()
        {
            return _adminBorrowService.ToDealWithBorrows();
        }

        [HttpGet("FinishedBorrows")]
        public IEnumerable<UserAdmiBorrowDto> FinishedBorrows()
        {
            return _adminBorrowService.FinishedBorrows();
        }

        [HttpGet("OpenBorrows")]
        public IEnumerable<UserAdmiBorrowDto> OpenBorrows()
        {
            return _adminBorrowService.OpenBorrows();
        }


    }
}
