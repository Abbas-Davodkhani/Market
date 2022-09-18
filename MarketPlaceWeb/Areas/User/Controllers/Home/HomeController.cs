using Microsoft.AspNetCore.Mvc;
using MarketPlaceWeb.Areas.User.Controllers.Base;

namespace MarketPlaceWeb.Areas.User.Controllers.Home
{
    public class HomeController : UserBaseController
    {
        #region Constructore
        #endregion
        #region Dashboard
        [HttpGet]
        [Route("")]
        public IActionResult Dashboard()
        {
            return View();
        }
        #endregion
    }
}
