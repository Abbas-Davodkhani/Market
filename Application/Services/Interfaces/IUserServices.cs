using DataLayer.DTOs.Account;
using DataLayer.Entities.Account;
using System;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface IUserServices : IAsyncDisposable
    {
        Task<RegisterUserResult> RegisterAsync(RegisterUserDTO regiser);
        Task<bool> IsUserExsistByMobileNumberAsync(string mobileNumber);
        Task<User> GetUserByMobileAsync(string mobile);
        Task<LoginUserResult> GetUserForLogin(LoginUserDTO loginUser);
        Task<ForgotPasswordUserResult> RecoverUserPassword(ForgotPasswordDTO forgotPasswordDTO);
        Task<bool> ActiveMobileAsync(ActivateMobileDTO activateMobileDTO);
        Task<bool> ChangeUserPasswordAsync(ChangePasswordDTO changePasswordDTO , long currentUserId);
        Task<EditUserProfileDTO> GetProfileForEdit(long userId);
        Task<EditUserProfileResult> EditUserProfile(EditUserProfileDTO profile, long userId);
    }
}
