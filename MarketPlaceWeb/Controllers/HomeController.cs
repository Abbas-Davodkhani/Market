using Application.Services.Interfaces;
using DataLayer.DTOs.Contacts;
using DataLayer.Entities.Site;
using GoogleReCaptcha.V3.Interface;
using MarketPlaceWeb.Utitlities.Extentions;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MarketPlaceWeb.Controllers
{
    public class HomeController : SiteBaseController
    {
        #region Constructor
        private readonly IContactUsService _contactUsService;
        private readonly ICaptchaValidator _captchaValidator;
        private readonly ISiteService _siteService;
        public HomeController(IContactUsService contactUsService, ICaptchaValidator captchaValidator , 
            ISiteService siteService)
        {
            _contactUsService = contactUsService;
            _captchaValidator = captchaValidator;
            _siteService = siteService; 
        }

        #endregion
        #region Index
        public async Task<IActionResult> Index()
        {
            ViewBag.Banners = await _siteService.GetSiteBannersByPlacementAsync(new List<BannerPlacement>
            {
                BannerPlacement.Home_1 , BannerPlacement.Home_2, BannerPlacement.Home_3
            });
            return View();
        }
        #endregion
        #region ContactUs
        [HttpGet("Contact-Us")]
        public IActionResult ContactUs()
        {
            return View();
        }
        [HttpPost("Contact-Us"), ValidateAntiForgeryToken]
        public async Task<IActionResult> ContactUs(CreateContactUsDTO contactUsDTO)
        {
            try
            {
                if (!await _captchaValidator.IsCaptchaPassedAsync(contactUsDTO.Captcah))
                {
                    TempData[ErrorMessage] = "کد کپچا تایید نشده است";
                    return View(contactUsDTO);
                }
            }
            catch (System.Exception ex)
            {
                var message = ex.Message;
                throw;
            }

            if (ModelState.IsValid)
            {
                var userIp = HttpContext.GetUserIp();
                await _contactUsService.CreateContactUs(contactUsDTO, userIp, User.GetUserId());
                TempData[SuccessMessage] = "پیام شما با موفقیت ارسال شد";
                return RedirectToAction(nameof(ContactUs));

            }
            return View(contactUsDTO);
        }
        #endregion
        #region AboutUs
        [HttpGet("About-Us")]
        public async Task<IActionResult> AboutUs()
        {
            var siteSetting = await _siteService.GetDefaultSiteSettingAsync();
            return View(siteSetting);
        }
        #endregion
    }
}