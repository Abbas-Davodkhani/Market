using Application.Services.Interfaces;
using DataLayer.DTOs;
using DataLayer.DTOs.Common;
using MarketPlaceWeb.Areas.Admin.Controllers.Base;
using MarketPlaceWeb.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MarketPlaceWeb.Areas.Admin.Controllers
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

        #region StoreRequests

        [HttpGet("store-requests")]
        public async Task<IActionResult> StoreRequests(FilterRequestStoreDTo filter)
        {
            filter.TakeEntity = 5;
            return View(await _storeService.FilterStores(filter));
        }

        #endregion

        #region AcceptStoreRequest
       
        public async Task<IActionResult> AcceptStoreRequest(long requestId)
        {
            var result = await _storeService.AcceptSellerRequest(requestId);

            if (result)
            {
                return JsonResponseStatus.SendStatus(
                    JsonResponseStatusType.Success,
                    "درخواست مورد نظر با موفقیت تایید شد",
                    null);
            }

            return JsonResponseStatus.SendStatus(JsonResponseStatusType.Danger,
                "اطلاعاتی با این مشخصات یافت نشد", null);
        }

        #endregion

        #region RejectStoreRequest

        public async Task<IActionResult> RejectStoreRequest(RejectItemDTO reject)
        {
            var result = await _storeService.RejectSellerRequest(reject);

            if (result)
            {
                return JsonResponseStatus.SendStatus(
                    JsonResponseStatusType.Success,
                    "درخواست مورد نظر با موفقیت تایید شد",
                    reject);
            }

            return JsonResponseStatus.SendStatus(JsonResponseStatusType.Danger,
                "اطلاعاتی با این مشخصات یافت نشد", null);
        }

        #endregion

    }
}
