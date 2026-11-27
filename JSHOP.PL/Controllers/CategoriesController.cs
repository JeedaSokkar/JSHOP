
using JSHOP.BLL.Services.Interfaces;
using JSHOP.DAL.Dto.Request;
using JSHOP.PL.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace JSHOP.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly ICategoryService _categoryService;

        public CategoriesController(
            IStringLocalizer<SharedResources> localizer,
            ICategoryService categoryService)
        {
            _localizer = localizer;
            _categoryService = categoryService;
        }

        // GET: api/Categories
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _categoryService.GetAllCategories();

            if (!result.IsSuccess)
            {
                return BadRequest(new
                {
                    message = result.Error?.Message
                });
            }

            return Ok(new
            {
                message = _localizer["success"].Value,
                data = result.Data
            });
        }

        // POST: api/Categories
        [HttpPost]
        public async Task<IActionResult> Create(CategoryRequest request)
        {
            var result = await _categoryService.CreateCategory(request);

            if (!result.IsSuccess)
            {
                return BadRequest(new
                {
                    message = result.Error?.Message
                });
            }

            return Ok(new
            {
                message = _localizer["success"].Value,
                data = result.Data
            });
        }

        // GET: api/Categories/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _categoryService.GetCategory(
                category => category.Id == id
            );

            if (!result.IsSuccess)
            {
                return NotFound(new
                {
                    message = result.Error?.Message
                });
            }

            return Ok(new
            {
                message = _localizer["success"].Value,
                data = result.Data
            });
        }

        // DELETE: api/Categories/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _categoryService.DeleteCategory(id);

            if (!result.IsSuccess)
            {
                return BadRequest(new
                {
                    message = result.Error?.Message
                });
            }

            return Ok(new
            {
                message = _localizer["success"].Value
            });
        }
    }
}
