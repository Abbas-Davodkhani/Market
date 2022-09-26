using DataLayer.DTOs.Products;
using System;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface IProductService : IAsyncDisposable
    {
        #region Products
        Task<ProductFilterDTO> FilterProducts(ProductFilterDTO filter);
        #endregion
    }
}