using DataLayer.DTOs.Account;
using MarketPlaceWeb.Areas.User.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlaceWeb.Areas.User.Controllers.Account
{
    public class AccountController : UserBaseController
    {
        [HttpGet]
        [Route("change-password")]
        public IActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        [Route("change-password")]
        public IActionResult ChangePassword(ChangePasswordDTO password)
        {
            return View();
        }
    }
}
