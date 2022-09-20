using Application.Services.Interfaces;
using DataLayer.DTOs.Account;
using MarketPlaceWeb.Areas.User.Controllers.Base;
using MarketPlaceWeb.Utitlities.Extentions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MarketPlaceWeb.Areas.User.Controllers.Account
{
    public class AccountController : UserBaseController
    {
        #region Constructor
        private readonly IUserServices _userService;
        public AccountController(IUserServices userService)
        {
            _userService = userService;
        }
        #endregion
        #region ChangePassword
        [HttpGet]
        [Route("change-password")]
        public IActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        [Route("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDTO password)
        {
            if(ModelState.IsValid)
            {
                var res = await _userService.ChangeUserPasswordAsync(password , User.GetUserId());
                if (res)
                {
                    TempData[SuccessMessage] = "کلمه ی عبور شما تغییر یافت";
                    TempData[InfoMessage] = "لطفا جهت تکمیل فرایند تغییر کلمه ی عبور ، مجددا وارد سایت شوید";
                    await HttpContext.SignOutAsync();
                    return RedirectToAction("Login", "Account", new { area = "" });
                }
                else
                {
                    TempData[ErrorMessage] = "لطفا از کلمه ی عبور جدیدی استفاده کنید";
                    ModelState.AddModelError("NewPassword", "لطفا از کلمه ی عبور جدیدی استفاده کنید");
                }
            }
            return View();
        }
        #endregion
        #region EditProfile
        [HttpGet("edit-profile")]
        public async Task<IActionResult> EditProfile()
        {
            var userProfile = await _userService.GetProfileForEdit(User.GetUserId());
            if (userProfile == null) return NotFound();
            return View(userProfile);
        }

        [HttpPost("edit-profile"), ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(EditUserProfileDTO profile, IFormFile avatarImage)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.EditUserProfile(profile, User.GetUserId() , avatarImage);
                switch (result)
                {
                    case EditUserProfileResult.IsBlocked:
                        TempData[ErrorMessage] = "حساب کاربری شما بلاک شده است";
                        break;
                    case EditUserProfileResult.IsNotActive:
                        TempData[ErrorMessage] = "حساب کاربری شما فعال نشده است";
                        break;
                    case EditUserProfileResult.NotFound:
                        TempData[ErrorMessage] = "کاربری با مشصخات وارد شده یافت نشد";
                        break;
                    case EditUserProfileResult.Success:
                        TempData[SuccessMessage] = $"جناب {profile.FirstName} {profile.LastName}، پروفایل شما با موفقیت ویرایش شد";
                        return RedirectToAction("EditProfile");
                }

            }

            return View(profile);
        }
        #endregion
    }
}
