
using JSHOP.BLL.Common.Results;
using JSHOP.DAL.Dto.Request;
using JSHOP.DAL.Dto.Response;

namespace JSHOP.BLL.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<Result> RegisterAsync(RegisterRequest request);

        Task<Result<LoginResponse>> LoginAsync(LoginRequest request);

        Task<Result> ConfirmEmail(ConfirmEmailRequest request);
    }
}
