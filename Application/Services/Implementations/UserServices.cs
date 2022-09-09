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
                    MobileActiveCode =  Guid.NewGuid().ToString("N")
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

        #endregion
        #region Dispose
        public async ValueTask DisposeAsync()
        {
            await _userRepositroy.DisposeAsync();
        }

        Task<RegisterUserResult> IUserServices.RegisterAsync(RegisterUserDTO regiser)
        {
            throw new NotImplementedException();
        }

        Task<bool> IUserServices.IsUserExsistByMobileNumberAsync(string mobileNumber)
        {
            throw new NotImplementedException();
        }

        Task<User> IUserServices.GetUserByMobileAsync(string mobile)
        {
            throw new NotImplementedException();
        }

        Task<LoginUserResult> IUserServices.GetUserForLogin(LoginUserDTO loginUser)
        {
            throw new NotImplementedException();
        }

        Task<ForgotPasswordUserResult> IUserServices.RecoverUserPassword(ForgotPasswordDTO forgotPasswordDTO)
        {
            throw new NotImplementedException();
        }


        #endregion
    }
}
