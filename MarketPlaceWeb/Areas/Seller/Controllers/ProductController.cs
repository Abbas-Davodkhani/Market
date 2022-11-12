using Application.Services.Interfaces;
using DataLayer.DTOs.Products;
using MarketPlaceWeb.Http;
using MarketPlaceWeb.Utitlities.Extentions;
using Microsoft.AspNetCore.Http;
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

        [HttpGet("list")]
        public async Task<IActionResult> Index(ProductFilterDTO filter)
        {
            var store = await _storeService.GetLastActiveSellerByUserId(User.GetUserId());
            filter.StoreId = store.Id;
            filter.FilterProductState = FilterProductState.All;
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
                var store = await _storeService.GetLastActiveSellerByUserId(User.GetUserId());
                var res = await _productService.CreateProduct(product,store.Id,productImage);



                switch (res)
                {
                    case CreateProductResult.HasNoImage:
                        TempData[WarningMessage] = "لطفا تصویر محصول را وارد نمایید";
                        break;
                    case CreateProductResult.Error:
                        TempData[ErrorMessage] = "عملیات ثبت محصول با خطا مواجه شد";
                        break;
                    case CreateProductResult.Success:
                        TempData[SuccessMessage] = $"محصول مورد نظر با عنوان {product.Title} با موفقیت ثبت شد";
                        return RedirectToAction("Index");
                }
            }
            ViewBag.Categories = await _productService.GetAllActiveProductCategoriesAsync();
            return View(product);
        }
        #endregion
        #region edit product

        [HttpGet("edit-product/{productId}")]
        public async Task<IActionResult> EditProduct(long productId)
        {
            var product = await _productService.GetProductForEdit(productId);
            if (product == null) return NotFound();
            ViewBag.Categories = await _productService.GetAllActiveProductCategoriesAsync();
            return View(product);
        }

        [HttpPost("edit-product/{productId}"), ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProduct(EditProductDTO product, IFormFile productImage)
        {
            if (ModelState.IsValid)
            {
                var res = await _productService.EditSellerProduct(product, User.GetUserId(), productImage);

                switch (res)
                {
                    case EditProductResult.NotForUser:
                        TempData[ErrorMessage] = "در ویرایش اطلاعات خطایی رخ داد";
                        break;
                    case EditProductResult.NotFound:
                        TempData[WarningMessage] = "اطلاعات وارد شده یافت نشد";
                        break;
                    case EditProductResult.Success:
                        TempData[SuccessMessage] = "عملیات با موفقیت انجام شد";
                        return RedirectToAction("Index");
                }
            }

            ViewBag.Categories = await _productService.GetAllActiveProductCategoriesAsync();
            return View(product);
        }

        #endregion
        #region product categories
        public async Task<ActionResult> GetProductCategoriesByParent(int parentId)
        {
            return JsonResponseStatus.SendStatus(JsonResponseStatusType.Success , "" , parentId);
        }
        #endregion
    }
}
