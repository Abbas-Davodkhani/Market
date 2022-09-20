using Application.Services.Interfaces;
using DataLayer.DTOs.Account;
using DataLayer.Entities.Account;
using DataLayer.Repositories.GenericRepostitory;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services.Implementations
{
    public class UserServices : IUserServices
    {

        #region Constructore
        private readonly IGenericRepository<User> _userRepositroy;
        private readonly IPasswordHelper _passwordHelper;
        public UserServices(IGenericRepository<User> userRepository , IPasswordHelper passwordHelper)
        {
            _userRepositroy = userRepository;
            _passwordHelper = passwordHelper;
        }


        #endregion

        #region Account
        public async Task<RegisterUserResult> RegisterAsync(RegisterUserDTO regiser)
        {
            if (!await IsUserExsistByMobileNumberAsync(regiser.Mobile))
            {
                User user = new User
                {
                    Mobile = regiser.Mobile,
                    FirstName = regiser.FirstName,
                    LastName = regiser.LastName,
                    IsMobileActive = false,
                    Password = _passwordHelper.EncodePasswordMd5(regiser.Password),
                    MobileActiveCode =  new Random().Next(1234 , 9999).ToString() , 
                    EmailActiveCode = "000"
                    
                };
                await _userRepositroy.AddEntityAsync(user);
                await _userRepositroy.SaveChangesAsync();
                //TODO Send Mobile Active Code

                return RegisterUserResult.Success;
            }
            return RegisterUserResult.MobileExsist;
        }
        public async Task<bool> IsUserExsistByMobileNumberAsync(string mobileNumber)
        {
            return await _userRepositroy.GetQuery().AsQueryable().AnyAsync(x => x.Mobile == mobileNumber);
        }


        public async Task<User> GetUserByMobileAsync(string mobile)
        {
            return await _userRepositroy.GetQuery().AsQueryable().SingleOrDefaultAsync(s => s.Mobile == mobile);
        }
        public async Task<LoginUserResult> GetUserForLogin(LoginUserDTO loginUser)
        {
            try
            {
                var user = await _userRepositroy.GetQuery().AsQueryable().SingleOrDefaultAsync(x => x.Mobile == loginUser.Mobile);
                if (user == null) return LoginUserResult.NotFound;
                if (!user.IsMobileActive) return LoginUserResult.NotActivated;
                if (user.Password != _passwordHelper.EncodePasswordMd5(loginUser.Password)) return LoginUserResult.NotFound;
            return LoginUserResult.Success;
            }
            catch (Exception ex)
            {
                return LoginUserResult.NotFound;            
            }
            
        }

        public async Task<ForgotPasswordUserResult> RecoverUserPassword(ForgotPasswordDTO forgotPasswordDTO)
        {
            var user = await _userRepositroy.GetQuery().SingleOrDefaultAsync(x => x.Mobile == forgotPasswordDTO.Mobile);
            if (user == null) return ForgotPasswordUserResult.NotFound;
            string newPassword = new Random().Next(1234 , 6789).ToString();
            user.Password = _passwordHelper.EncodePasswordMd5(newPassword);
            return ForgotPasswordUserResult.Success;
        }

        public async Task<bool> ActiveMobileAsync(ActivateMobileDTO activateMobileDTO)
        {
            var user = await GetUserByMobileAsync(activateMobileDTO.Mobile);
            if(user != null)
            {
                if(user.MobileActiveCode == activateMobileDTO.MobileActiveCode)
                {
                    user.IsMobileActive = true;
                    user.MobileActiveCode = new Random().Next(1000, 9999).ToString();
                    await _userRepositroy.SaveChangesAsync();
                    return true;
                }   
            }
            return false;
        }

        public async Task<bool> ChangeUserPasswordAsync(ChangePasswordDTO changePasswordDTO, long currentUserId)
        {
            var user = await _userRepositroy.GetByIdAsync(currentUserId);
            if(user != null)
            {
                string newPassword = _passwordHelper.EncodePasswordMd5(changePasswordDTO.NewPassword);
                if(newPassword != user.Password)
                {
                    user.Password = newPassword;
                    _userRepositroy.UpdateEntity(user);
                    await _userRepositroy.SaveChangesAsync();
                    return true;
                }
            }
            return false;
        }
        public async Task<EditUserProfileDTO> GetProfileForEdit(long userId)
        {
            var user = await _userRepositroy.GetByIdAsync(userId);
            if(user == null) return null;

            return new EditUserProfileDTO
            {
               FirstName = user.FirstName,
               LastName = user.LastName,
               Avatar = user.Avatar,
            };
        }
        public async Task<EditUserProfileResult> EditUserProfile(EditUserProfileDTO profile, long userId)
        {
            var user = await _userRepositroy.GetByIdAsync(userId);
            if (user == null) return EditUserProfileResult.NotFound;

            if (user.IsBlocked) return EditUserProfileResult.IsBlocked;
            if (!user.IsMobileActive) return EditUserProfileResult.IsNotActive;

            user.FirstName = profile.FirstName;
            user.LastName = profile.LastName;
            _userRepositroy.UpdateEntity(user);
            await _userRepositroy.SaveChangesAsync();

            return EditUserProfileResult.Success;
        }
        #endregion

        #region Dispose
        public async ValueTask DisposeAsync()
        {
            await _userRepositroy.DisposeAsync();
        }
        #endregion
    }
}
