using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ZID.Automat.Application;
using ZID.Automat.Domain.Models;
using ZID.Automat.Dto;
using ZID.Automat.Repository;
using System;

namespace ZID.Automat.Api.Controllers.User
{
    [ApiController]
    [AllowAnonymous]
    [Route("[controller]")]
    public class UItemController : Controller
    {
        private  readonly IAllDisplayItems _allDisplayItemsService;
        private  readonly IPrevBorrowedDisplayItemsOfUser _prevBorrowedDisplayItemsOfUser;
        public UItemController(IAllDisplayItems allDisplayItemsService, IPrevBorrowedDisplayItemsOfUser prevBorrowedDisplayItemsOfUser)
        {
            _allDisplayItemsService = allDisplayItemsService;
            _prevBorrowedDisplayItemsOfUser = prevBorrowedDisplayItemsOfUser;
        }
        
        [HttpGet("GetAllDsplayItems")]
        public IReadOnlyList<ItemDisplayDto> getAllItems()
        {
            return _allDisplayItemsService.AllDisplayItems();
        }

        [HttpGet("GetAllPrevBorrowedItems")]
        public IReadOnlyList<ItemDisplayDto> GetAllPrevBorrowedItems()
        {
            //HttpContext.User.Claims.First(claim=>claim.)
            return _prevBorrowedDisplayItemsOfUser.PrevBorrowedDisplayItemsUser(0);
        }
    }
}
