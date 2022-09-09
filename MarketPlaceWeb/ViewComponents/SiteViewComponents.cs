using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MarketPlaceWeb.ViewComponents
{
    #region Header
    public class SiteHeaderViewComponent : ViewComponent
    {
        private readonly ISiteService _siteService;
        public SiteHeaderViewComponent(ISiteService siteService)
        {
            _siteService = siteService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewBag.SiteSetting = await _siteService.GetDefaultSiteSettingAsync();
            return View("SiteHeader");
        }
          
    }
    #endregion 


    #region Footer
    public class SiteFooterViewComponent : ViewComponent
    {
        private readonly ISiteService _siteService;
        public SiteFooterViewComponent(ISiteService siteService)
        {
            _siteService = siteService;
        }
        #region Header
        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewBag.SiteSetting = await _siteService.GetDefaultSiteSettingAsync();
            return View("SiteFooter");
        }
        #endregion   
    }
    #endregion 
}
