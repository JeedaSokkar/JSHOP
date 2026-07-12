using JSHOP.DAL.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JSHOP.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        ApplicationDbContexet _context;

        public CategoriesController(ApplicationDbContexet context)
        {
            _context = context;
        }
        [HttpGet]
        //test connection to database
        public IActionResult Index()
        {
            try
            {
                if (_context.Database.CanConnect())
                {

                    return Ok("done");
                }
                else
                {
                    return StatusCode(500, "error");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
