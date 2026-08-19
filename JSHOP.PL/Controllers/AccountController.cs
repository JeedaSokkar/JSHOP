using JSHOP.BLL.Services;
using JSHOP.DAL.Dto.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JSHOP.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly  IAuthenticationService _authenticationService;
        public AccountController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("Register")]

        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var result=await _authenticationService.RegisterAsync(request);
            return Ok(result);
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var result = await _authenticationService.LoginAsync(request);
            return Ok(result);
        }
    }
}
