using Microsoft.AspNetCore.Mvc;
using ZID.Automat.Application.Admin;
using ZID.Automat.Dto.Models;
using ZID.Automat.Dto.Models.Analytics.User;

namespace ZID.Automat.Api.Controllers.Admin
{
    [Route("[controller]")]
    [ApiController]
    public class AConfCategoriesController: ControllerBase
    {
        private readonly IConfCategoriesService _confCategoriesService;

        public AConfCategoriesController(IConfCategoriesService confCategoriesService)
        {
            _confCategoriesService = confCategoriesService;
        }

        [HttpGet("GetConfCategories")]
        public IEnumerable<CategoryUpdateDto> getConfCategories()
        {
            return _confCategoriesService.AllCategories();
        }

        [HttpPatch("UpdateConfCategory")]
        public void UpdateCategory([FromBody] CategoryUpdateDto category)
        {
            _confCategoriesService.UpdateCategory(category);
        }

        [HttpPost("AddCategory")]
        public void AddCategory([FromBody] CategoryAddDto category)
        {
            _confCategoriesService.AddCategory(category);
        }
    }
}
