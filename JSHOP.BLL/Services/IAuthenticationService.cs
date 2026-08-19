using JSHOP.DAL.Dto.Request;
using JSHOP.DAL.Dto.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSHOP.BLL.Services
{
   public interface IAuthenticationService
    {
        Task<RegisterResponse> RegisterAsync(RegisterRequest request);
        Task<LoginResponse> LoginAsync(LoginRequest request);
    }
}
