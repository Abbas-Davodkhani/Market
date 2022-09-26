using Microsoft.AspNetCore.Mvc;

namespace MarketPlaceWeb.Web.Areas.Seller.Controllers
{
    public class HomeController : SellerBaseController
    {
        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
