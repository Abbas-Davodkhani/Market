using Application.Services.Interfaces;
using DataLayer.DTOs.Products;
using MarketPlaceWeb.Utitlities.Extentions;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MarketPlaceWeb.Web.Areas.Seller.Controllers
{
    public class ProductController : SellerBaseController
    {
        #region constructor

        private readonly IProductService _productService;
        private readonly IStoreService _storeService;
        public ProductController(IProductService productService , IStoreService storeService)
        {
            _productService = productService;
            _storeService = storeService;
        }

        #endregion

        #region List

        [HttpGet("products")]
        public async Task<IActionResult> Index(ProductFilterDTO filter)
        {
            var store = await _storeService.GetLastActiveSellerByUserId(User.GetUserId());
            filter.StoreId = store.Id;
            filter.FilterProductState = FilterProductState.Active;
            return View(await _productService.FilterProducts(filter));
        }

        #endregion
    }
}
