using JSHOP.BLL.Services.Classes;
using JSHOP.BLL.Services.Interfaces;
using JSHOP.DAL.Dto.Request;
using JSHOP.PL.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace JSHOP.PL.Controllers
{

    [Route("api/[controller]")]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IProductService _productService;
        public ProductsController(
           IStringLocalizer<SharedResources> localizer,
           IProductService productService)
        {
            _localizer = localizer;
            _productService = productService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromForm] ProductRequest request)
        {
         
            var result = await _productService.CreateProduct(request);
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
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var result = await _productService.GetAllProducts();

            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
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
    }
}
