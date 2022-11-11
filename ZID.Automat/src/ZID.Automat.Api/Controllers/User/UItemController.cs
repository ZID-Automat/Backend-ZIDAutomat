using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZID.Automat.Application;
using ZID.Automat.Domain.Models;
using ZID.Automat.Dto.Models;
using ZID.Automat.Repository;

namespace ZID.Automat.Api.Controllers.User
{
    [ApiController]
    [AllowAnonymous]
    [Route("[controller]")]
    public class UItemController : Controller
    {
        private readonly IItemService _itemService;
        public UItemController(IItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpGet("getAllItems")]
        public IReadOnlyList<ItemDisplayDto> getAllItems()
        {
            return _itemService.AllDisplayItems();
        }
        
        [HttpGet("getPrevBorrowed")]
        public IReadOnlyList<ItemDisplayDto> getPrevBorrowed()
        {
            return _itemService.PrevBorrowedDisplayItemsUser(User.Claims.First(c=>c.Issuer == "Name").Value);
        }
    }
}
