using Application.Services.Interfaces;
using DataLayer.DTOs.Contacts;
using GoogleReCaptcha.V3.Interface;
using MarketPlaceWeb.Utitlities.Extentions;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MarketPlaceWeb.Controllers
{
    public class HomeController : SiteBaseController
    {
        #region Constructor
        private readonly IContactUsService _contactUsService;
        private readonly ICaptchaValidator _captchaValidator;
        public HomeController(IContactUsService contactUsService, ICaptchaValidator captchaValidator)
        {
            _contactUsService = contactUsService;
            _captchaValidator = captchaValidator;
        }

        #endregion
        #region Index
        public IActionResult Index()
        {
            TempData[ErrorMessage] = "Test error message";
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
                await _contactUsService.CreateContactUs(contactUsDTO , userIp , User.GetUserId());
                TempData[SuccessMessage] = "پیام شما با موفقیت ارسال شد";
                return RedirectToAction(nameof(ContactUs));

            }
            return View(contactUsDTO);
        }
        #endregion

    }
}