using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZID.Automat.Application.Admin;
using ZID.Automat.Dto.Models.Items;

namespace ZID.Automat.Api.Controllers.Admin
{
    [ApiController]
    [Authorize(Roles = "Admin")]
    [Route("[controller]")]
    public class AItemController : ControllerBase
    {
        private readonly IAdminItemService _adminItemService;
        public AItemController(IAdminItemService adminItemService)
        {
            _adminItemService = adminItemService;
        }

        [HttpGet("GetAll")]
        public IEnumerable<ItemGetAllDto> GetAll()
        {
            return _adminItemService.GetAllItems();
        }

        [HttpPost("SetItemPosition")]
        public void SetItemPosition(ItemChangeLocationDto itemChangeLocationDto)
        {
            _adminItemService.SetItemPosition(itemChangeLocationDto);
        }

        [HttpGet("ItemAdminDetailed")]
        public ItemAdminDetailedDto ItemDetailedAdmin(int id)
        {
            return _adminItemService.ItemDetailedGet(id);
        }


        [HttpPost("ItemDetailedAdminAdd")]
        public void ItemDetailedAdminAdd(ItemAdminUpdateAdd data)
        {
            _adminItemService.AddItemDetailed(data);
        }

        [HttpPatch("ItemDetailedAdminUpdate")]
        public void ItemDetailedAdminUpdate(ItemAdminUpdateAdd data)
        {
            _adminItemService.UpdateItemDetailed(data);
        }

        [HttpPost("AddItemInstace")]
        public void AddItemInstace([FromQuery]int id)
        {
            _adminItemService.AddItemInstace(id);
        }

        [HttpGet("GetItemInstances")]
        public int GetItemInstances([FromQuery] int id)
        {
            return _adminItemService.GetItemInstances(id);
        }
    }
}
