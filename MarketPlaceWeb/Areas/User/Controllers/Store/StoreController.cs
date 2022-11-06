using Application.Services.Interfaces;
using DataLayer.DTOs.Store;
using MarketPlaceWeb.Areas.User.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MarketPlaceWeb.Utitlities.Extentions;
using DataLayer.DTOs;
using DataLayer.DTOs.Stores;

namespace MarketPlaceWeb.Areas.User.Controllers
{
    public class StoreController : AdminBaseController
    {
        #region Constructor
        private readonly IStoreService _storeService;

        public StoreController(IStoreService storeService)
        {
            _storeService = storeService;
        }
        #endregion
        #region Request
        [HttpGet("request-store-panel")]
        public IActionResult RequestStorePanel()
        {
            return View();
        }
        [HttpPost("request-store-panel") , ValidateAntiForgeryToken]
        public async Task<IActionResult> RequestStorePanel(RequestStorePanelDTO requestStore)
        {
            if(ModelState.IsValid)
            {
                var res = await _storeService.AddNewStoreRequest(requestStore, User.GetUserId());
                switch (res)
                {
                    case RequestStorePanelResult.HasNotPermission:
                        TempData[ErrorMessage] = "شما دسترسی لازم جهت انجام فرایند مورد نظر را ندارید";
                        break;
                    case RequestStorePanelResult.HasUnderProgressRequest:
                        TempData[WarningMessage] = "درخواست های قبلی شما در حال پیگیری می باشند";
                        TempData[InfoMessage] = "در حال حاضر نمیتوانید درخواست جدیدی ثبت کنید";
                        break;
                    case RequestStorePanelResult.Success:
                        TempData[SuccessMessage] = "درخواست شما با موفقیت ثبت شد";
                        TempData[InfoMessage] = "فرایند تایید اطلاعات شما در حال پیگیری می باشد";
                        return RedirectToAction("RequestStorePanel");
                }
            }
            return View(requestStore);
        }
        #endregion
        #region StoreRequests

        [HttpGet("store-requests")]
        public async Task<IActionResult> StoreRequests(FilterRequestStoreDTo filter)
        {
            filter.TakeEntity = 1;
            filter.UserId = User.GetUserId();
            filter.FilterStoreState = FilterStoreState.All;

            return View(await _storeService.FilterStores(filter));
        }

        #endregion
        #region EditeStoreRequest
        [HttpGet("edit-store-request/{id}")]
        public async Task<IActionResult> EditStoreRequest(long id)
        {
            var requestSeller = await _storeService.GetEditStoreRequestForEdit(id, User.GetUserId());
            if (requestSeller == null) return NotFound();


            return View(requestSeller);
        }

        [HttpPost("edit-store-request/{id}") , ValidateAntiForgeryToken]
        public async Task<IActionResult> EditStoreRequest(EditRequestStoreDTO request)
        {
            if(ModelState.IsValid)
            {
                var res = await _storeService.EditStoreRequest(request, User.GetUserId());
                switch (res)
                {
                    case EditRequestStoreResult.NotFound:
                        TempData[ErrorMessage] = "اطلاعات مورد نظر یافت نشد";
                        break;
                    case EditRequestStoreResult.Success:
                        TempData[SuccessMessage] = "اطلاعات مورد نظر با موفقیت ویرایش شد";
                        TempData[InfoMessage] = "فرآیند تایید اطلاعات از سر گرفته شد";
                        return RedirectToAction("StoreRequests");
                }
            }
            return View(request);
        }
        #endregion
    }
}
