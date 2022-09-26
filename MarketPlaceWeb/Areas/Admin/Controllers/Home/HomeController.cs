using Microsoft.AspNetCore.Mvc;
using MarketPlaceWeb.Areas.Admin.Controllers.Base;

namespace MarketPlaceWeb.Areas.Admin.Controllers.Home
{
    public class HomeController : AdminBaseController
    {
        #region Constructore
        #endregion
        #region Index
        [HttpGet]
       
        public IActionResult Index()
        {
            return View();
        }
        #endregion
    }
}
