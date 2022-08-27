using Microsoft.AspNetCore.Mvc;


namespace MarketPlaceWeb.Controllers
{
    public class HomeController : SiteBaseController
    {
        public IActionResult Index()
        {
            TempData[ErrorMessage] = "Test error message";
            return View();
        }     
    }
}