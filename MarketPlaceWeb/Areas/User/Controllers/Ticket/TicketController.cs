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
        [HttpGet("tickets")]
        public async Task<IActionResult> Index(FilterTicketDTO filter)
        {
            filter.UserId = User.GetUserId();
            filter.FilterTicketState = FilterTicketState.NotDeleted;
            filter.OrderBy = FilterTicketOrder.CreateDate_DES;

            return View(await _contactServices.FilterTicket(filter));
        }
        #endregion
        #region AddTicket
        [HttpGet("add-ticket")]
        public async Task<IActionResult> AddTicket()
        {
            return View();
        }

        [HttpPost("add-ticket"), ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTicket(AddTicketDTO ticket)
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
        #region Ticket Details
        [HttpGet("tickets/{ticketId}")]
        public async Task<IActionResult> TicketDetails(long ticketId)
        {
            var ticket = await _contactServices.GetTicketForShowAsync(ticketId, User.GetUserId());
            if (ticket == null) return NotFound();
                

            return View(ticket);
        }
        #endregion
        #region AnswereTicket
        [HttpPost("answere-ticket")]
        public async Task<IActionResult> AnswereTicket(AnswerTicketDTO answere)
        {
            if (string.IsNullOrEmpty(answere.Text)) TempData[ErrorMessage] = "لطفا متن پیام را وارد کنید";
            if(ModelState.IsValid)
            {
                var res = await _contactServices.AnswereTicket(answere, User.GetUserId());
                switch(res)
                {
                    case AnswerTicketResult.NotForUser:
                        TempData[ErrorMessage] = "عدم دسترسی";
                        TempData[InfoMessage] = "در صورت تکرار این مورد ، دسترسی شما به صورت کلی از سیستم قطع خواهد شد";
                        return RedirectToAction("Index");
                    case AnswerTicketResult.NotFound:
                        TempData[WarningMessage] = "اطلاعات مورد نظر یافت نشد";
                        return RedirectToAction("Index");
                    case AnswerTicketResult.Success:
                        TempData[SuccessMessage] = "اطلاعات مورد نظر با موفقیت ثبت شد";
                        break;
                }
            }

            return RedirectToAction("TicketDetails" , "Ticket" , new {area="User" , ticketId = answere.Id});
        }
        #endregion
    }
}
