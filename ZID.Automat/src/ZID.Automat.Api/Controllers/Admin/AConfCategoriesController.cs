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
        public IEnumerable<CategoryDto> getConfCategories()
        {
            return _confCategoriesService.AllCategories();
        }

        [HttpPost("UpdateConfCategory")]
        public void AddConfCategory([FromBody] CategoryDto category)
        {
            _confCategoriesService.UpdateCategoryDescription(category.Id,category.Name);
        }
    }
}
