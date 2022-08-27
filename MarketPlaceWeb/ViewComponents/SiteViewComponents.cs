using Microsoft.AspNetCore.Mvc;

namespace MarketPlaceWeb.ViewComponents
{
    #region Header
    public class SiteHeaderViewComponent : ViewComponent
    {
       
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("SiteHeader");
        }
          
    }
    #endregion 


    #region Footer
    public class SiteFooterViewComponent : ViewComponent
    {
        #region Header
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("SiteFooter");
        }
        #endregion   
    }
    #endregion 
}
