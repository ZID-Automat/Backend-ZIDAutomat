using Microsoft.AspNetCore.Mvc;
using ZID.Automat.Application.Admin;
using ZID.Automat.Dto.Models.Items;

namespace ZID.Automat.Api.Controllers.Admin
{
    [Route("[controller]")]
    [ApiController]
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
    }
}
