using JSHOP.BLL.Services;
using JSHOP.DAL.Data;
using JSHOP.DAL.Dto.Request;
using JSHOP.DAL.Dto.Response;
using JSHOP.DAL.Models;
using JSHOP.PL.Resources;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace JSHOP.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
       
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly ICategoryService _categoryService;
        public CategoriesController( IStringLocalizer<SharedResources> localizer, ICategoryService categoryService)
        {
           
            _localizer = localizer;
            _categoryService = categoryService;
        }
        [HttpGet("")]
        public async Task< IActionResult> Index()
        {
            var categories= await _categoryService.GetAllCategories();
            return Ok(new { _localizer["success"].Value,categories});
        }
        [HttpPost("")]
       
        public async Task< IActionResult> Index(CategoryRequest request)
        {
            var response = await _categoryService.CreateCategory(request);
            return Ok(_localizer["success"].Value);
        }
    }
}
