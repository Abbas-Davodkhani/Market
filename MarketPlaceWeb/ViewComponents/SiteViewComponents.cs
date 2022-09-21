using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MarketPlaceWeb.ViewComponents
{
    #region Header
    public class SiteHeaderViewComponent : ViewComponent
    {
        private readonly ISiteService _siteService;
        private readonly IUserServices _userServices;
        public SiteHeaderViewComponent(ISiteService siteService, IUserServices userServices)
        {
            _siteService = siteService;
            _userServices = userServices;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewBag.SiteSetting = await _siteService.GetDefaultSiteSettingAsync();
            ViewBag.User = null;
            if(User.Identity.IsAuthenticated)
            {
                ViewBag.User = await _userServices.GetUserByMobileAsync(User.Identity.Name);
            }
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
    #region Slider
    public class HeaderSliderViewComponent : ViewComponent
    {
        private readonly ISiteService _siteService;
        public HeaderSliderViewComponent(ISiteService siteService)
        {
            _siteService = siteService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var sliders = await _siteService.GetAllActiveSlidrsAsync();
            return View("HeaderSlider" , sliders);
        }
    }
    #endregion
}
