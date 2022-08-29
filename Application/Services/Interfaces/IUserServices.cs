using DataLayer.DTOs.Account;
using DataLayer.Entities.Account;

namespace Application.Services.Interfaces
{
    public interface IUserServices : IAsyncDisposable
    {
        Task<RegisterUserResult> RegisterAsync(RegisterUserDTO regiser);
        Task<bool> IsUserExsistByMobileNumberAsync(string mobileNumber);
        Task<User> GetUserByMobileAsync(string mobile);
        Task<LoginUserResult> GetUserForLogin(LoginUserDTO loginUser);
        Task<ForgotPasswordUserResult> RecoverUserPassword(ForgotPasswordDTO forgotPasswordDTO); 
    }
}
