using DataLayer.DTOs.Common;
using DataLayer.DTOs.Products;
using DataLayer.Entities.Products;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface IProductService : IAsyncDisposable
    {
        #region Products
        Task<ProductFilterDTO> FilterProducts(ProductFilterDTO filter);
        Task<CreateProductResult> CreateProduct(CreateProductDTO product, long storeId, IFormFile productImage);
        Task<bool> AcceptSellerProduct(long productId);
        Task<bool> RejectSellerProduct(RejectItemDTO reject);
        Task<EditProductDTO> GetProductForEdit(long productId);
        Task RemoveAllProductSelectedCategories(long productId);
        Task RemoveAllProductSelectedColors(long productId);
        Task AddProductSelectedColors(long productId, List<CreateProductColorDTO> colors);
        Task AddProductSelectedCategories(long productId, List<long> selectedCategories);
        Task<EditProductResult> EditSellerProduct(EditProductDTO product, long userId, IFormFile productImage);
        #endregion
        #region ProductCategories
        Task<List<ProductCategory>> GetAllProductCategoriesByParentIdAsync(long? parentId);
        Task<List<ProductCategory>> GetAllActiveProductCategoriesAsync();
        #endregion
    }
}