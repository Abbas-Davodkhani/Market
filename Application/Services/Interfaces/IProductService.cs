using DataLayer.DTOs.Products;
using DataLayer.Entities.Products;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface IProductService : IAsyncDisposable
    {
        #region Products
        Task<ProductFilterDTO> FilterProducts(ProductFilterDTO filter);
        Task<CreateProductResult> CreateProduct(CreateProductDTO product, string imageName, long storeId);
        #endregion
        #region ProductCategories
        Task<List<ProductCategory>> GetAllProductCategoriesByParentIdAsync(long? parentId);
        Task<List<ProductCategory>> GetAllActiveProductCategoriesAsync();
        #endregion
    }
}