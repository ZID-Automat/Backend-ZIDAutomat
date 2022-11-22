using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;
using ZID.Automat.Application;
using ZID.Automat.Dto.Models;

namespace ZID.Automat.Api.Controllers.User
{
    [ApiController]
    [Authorize(Roles = "User")]
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
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claims = identity.Claims;
            var cl = claims.First(c => c.Type == "Name");
            return _itemService.PrevBorrowedDisplayItemsUser(cl.Value);
        }

        [HttpGet("getDetailedItem")]
        public ItemDetailedDto getDetailedItem(int ItemId)
        {
            return _itemService.DetailedItem(ItemId); 
        }
    }
}
