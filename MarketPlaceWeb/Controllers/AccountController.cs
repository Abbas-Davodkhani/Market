using Application.Services.Interfaces;
using DataLayer.DTOs.Account;
using GoogleReCaptcha.V3.Interface;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MarketPlaceWeb.Controllers
{
    public class AccountController : SiteBaseController
    {
        #region Constructore
        private readonly IUserServices _userServices;
        private readonly ICaptchaValidator _captchaValidator;
        public AccountController(IUserServices userServices , ICaptchaValidator captchaValidator)
        {
            _userServices = userServices;
            _captchaValidator = captchaValidator;   
        }
        #endregion


        #region Register
        [Route("register")]
        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated) return Redirect("/");
            return View();
        }
        [Route("regiser")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUserDTO register)
        {
            if(!await _captchaValidator.IsCaptchaPassedAsync(register.Captcah))
            {
                TempData[ErrorMessage] = "کد کپچا تایید نشده است";
                return View(register);
            }
            if(ModelState.IsValid)
            {
                var res = await _userServices.RegisterAsync(register);
                switch (res)
                {
                    case RegisterUserResult.MobileExsist:
                        TempData[ErrorMessage] = "موبایل تکراری می باشد";
                        ModelState.AddModelError("", "");
                        break;
                    case RegisterUserResult.Success:
                        TempData[SuccessMessage] = "ثبت نام با موفقیت انجام شد";
                        TempData[InfoMessage] = "پیامک فعال سازی برای شما ارسال شد";
                        break;
                }

            }
            return View(register);
        }
        #endregion

        #region Login
        [HttpGet]
        [Route("login")]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated) Redirect("/");
            return View();
        }
        [HttpPost]
        [Route("login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginUserDTO login)
        {   
            if(!await _captchaValidator.IsCaptchaPassedAsync(login.Captcah))
            {
                TempData[ErrorMessage] = "کذ کپچا تایید نشده است";
                return View(login);
            }
            if(ModelState.IsValid)
            {
                var result = await _userServices.GetUserForLogin(login);
                switch (result)
                {
                    case LoginUserResult.NotFound:
                        TempData[ErrorMessage] = "شماره موبایل یا کلمه ی عبور اشتباه است";
                        return View(login);
                        break;
                    case LoginUserResult.Success:
                        var user = await _userServices.GetUserByMobileAsync(login.Mobile);

                        ClaimsIdentity identity = new ClaimsIdentity(new[]
                        {
                            new Claim(ClaimTypes.Name , user.Mobile) ,
                            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                        }, CookieAuthenticationDefaults.AuthenticationScheme);

                        var princpal = new ClaimsPrincipal(identity);
                        var properties = new AuthenticationProperties
                        {
                            IsPersistent = login.RememberMe,
                        };

                        await HttpContext.SignInAsync(princpal);

                        TempData[SuccessMessage] = "با موفقیت وارد شدید";
                        return Redirect("/");
                        break;
                }
            }
            return View(login);
        }
        #endregion

        #region LogOut
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }
        #endregion
        #region ForgotPassword
        [HttpGet("forgot-pass")]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost("forgot-pass") , ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDTO forgot)
        {
            if (!await _captchaValidator.IsCaptchaPassedAsync(forgot.Captcah))
            {
                TempData[ErrorMessage] = "کد کپچای شما تایید نشد";
                return View(forgot);
            }
            if (ModelState.IsValid)
            {
                var result = await _userServices.RecoverUserPassword(forgot);
                switch (result)
                {
                    case ForgotPasswordUserResult.NotFound:
                        TempData[WarningMessage] = "کاربر مورد نظر یافت نشد";
                        break;
                    case ForgotPasswordUserResult.Success:
                        TempData[SuccessMessage] = "کلمه ی عبور جدید برای شما ارسال شد";
                        TempData[InfoMessage] = "لطفا پس از ورود به حساب کاربری ، کلمه ی عبور خود را تغییر دهید";
                        return RedirectToAction("Login");
                }
            }
            return View(forgot);
        }
        #endregion
    }
}
