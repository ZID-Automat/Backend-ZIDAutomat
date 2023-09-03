using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZID.Automat.Application;
using ZID.Automat.Dto.Models;

namespace ZID.Automat.Api.Controllers.Admin
{
    [Route("[controller]")]
    [ApiController]
    public class ABorrowController : ControllerBase
    {
        private readonly IABorrowService _borrowService;
        public ABorrowController(IABorrowService borrowService)
        {
            _borrowService = borrowService;
        }

        [HttpGet("GetBorrowedItems")]
        //[Authorize]
        public IActionResult GetBorrowedItems()
        {
            return Ok();
        }

        //Set Borrow Item to returned
        [HttpPatch("SetReturnedBorrowed")]
        public IActionResult SetReturnedBorrowed()
        {
            return Ok();
        }
    }
}
