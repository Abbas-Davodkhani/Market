using Application.Services.Interfaces;
using DataLayer.DTOs.Account;
using MarketPlaceWeb.Areas.User.Controllers.Base;
using MarketPlaceWeb.Utitlities.Extentions;
using Microsoft.AspNetCore.Authentication;
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
    }
}
