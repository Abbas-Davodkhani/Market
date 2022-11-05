using Application.Services.Interfaces;
using DataLayer.DTOs.Products;
using MarketPlaceWeb.Utitlities.Extentions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MarketPlaceWeb.Web.Areas.Seller.Controllers
{
    [Route("products")]
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

        [HttpGet("list")]
        public async Task<IActionResult> Index(ProductFilterDTO filter)
        {
            var store = await _storeService.GetLastActiveSellerByUserId(User.GetUserId());
            filter.StoreId = store.Id;
            filter.FilterProductState = FilterProductState.Active;
            return View(await _productService.FilterProducts(filter));
        }

        #endregion

        #region CreateProduct

        [HttpGet("create-product")]
        public async Task<IActionResult> CreateProduct()
        {
            ViewBag.Categories = await _productService.GetAllActiveProductCategoriesAsync();

            return View();
        }

        [HttpPost("create-product"), ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProduct(CreateProductDTO product, IFormFile productImage)
        {
            if (ModelState.IsValid)
            {
                // todo: create product
            }

            ViewBag.Categories = await _productService.GetAllActiveProductCategoriesAsync();
            return View(product);
        }

        #endregion
    }
}
