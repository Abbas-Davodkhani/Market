using Application.Services.Interfaces;
using DataLayer.DTOs.Contacts;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MarketPlaceWeb.Utitlities.Extentions;
using MarketPlaceWeb.Areas.User.Controllers.Base;

namespace MarketPlaceWeb.Areas.User.Controllers
{
    public class TicketController : UserBaseController
    {
        #region Constructor

        private readonly IContactUsService _contactServices;

        public TicketController(IContactUsService contactServices)
        {
            _contactServices = contactServices;
        }

        #endregion
        #region List
        public IActionResult Index()
        {
            return View();
        }
        #endregion
        #region AddTicket
        [HttpGet("add-ticket")]
        public async Task<IActionResult> AddTicket()
        {
            return View();
        }

        [HttpPost("add-ticket"), ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTicket(AddTicketViewModel ticket)
        {
            if (ModelState.IsValid)
            {
                var result = await _contactServices.AddTicket(ticket, User.GetUserId());
                switch (result)
                {
                    case AddTicketResult.Error:
                        TempData[ErrorMessage] = "عملیات با شکست مواجه شد";
                        break;
                    case AddTicketResult.Success:
                        TempData[SuccessMessage] = "تیکت شما با موفقیت ثبت شد";
                        TempData[InfoMessage] = "پاسخ شما به زودی ارسال خواهد شد";
                        return RedirectToAction("Index");
                }
            }

            return View(ticket);
        }
        #endregion
    }
}
