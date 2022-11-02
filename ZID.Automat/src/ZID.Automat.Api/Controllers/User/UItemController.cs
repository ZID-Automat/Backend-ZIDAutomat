using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZID.Automat.Domain.Models;
using ZID.Automat.Repository;

namespace ZID.Automat.Api.Controllers.User
{
    [ApiController]
    [AllowAnonymous]
    [Route("[controller]")]
    public class UItemController : Controller
    {
        private  readonly IItemRepository ItemRepository;
        public UItemController(IItemRepository itemRepository)
        {
            ItemRepository = itemRepository;
        }
        
        public List<Item> getAllItems()
        {
            return ItemRepository.getItemWithItemInstance();
        }
    }
}
