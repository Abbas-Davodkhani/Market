using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Application.Extensions;
using Application.Services.Interfaces;
using Application.Utils;
using DataLayer.DTOs.Common;
using DataLayer.DTOs.Paging;
using DataLayer.DTOs.Products;
using DataLayer.Entities.Products;
using DataLayer.Repositories.GenericRepostitory;
using MarketPlace.Application.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Implementations
{
    public class ProductService : IProductService
    {
        #region constructor

        private readonly IGenericRepository<Product> _productRepository;
        private readonly IGenericRepository<ProductCategory> _productCategoryRepository;
        private readonly IGenericRepository<ProductSelectedCategory> _productSelectedCategoryRepository;
        private readonly IGenericRepository<ProductColor> _productColorReporsitory;
        public ProductService(IGenericRepository<Product> productRepository, IGenericRepository<ProductCategory> productCategoryRepository,
            IGenericRepository<ProductSelectedCategory> productSelectedCategoryRepository , IGenericRepository<ProductColor> productColorReporsitory)
        {
            _productRepository = productRepository;
            _productCategoryRepository = productCategoryRepository;
            _productSelectedCategoryRepository = productSelectedCategoryRepository;
            _productColorReporsitory = productColorReporsitory;
        }

        #endregion

        #region products

        public async Task<ProductFilterDTO> FilterProducts(ProductFilterDTO filter)
        {
            var query = _productRepository.GetQuery().AsQueryable();

            #region State

            switch (filter.FilterProductState)
            {
                case FilterProductState.All:
                    break;
                case FilterProductState.Active:
                    query = query.Where(s => s.IsActive && s.ProductAcceptanceState == ProductAcceptanceState.Accepted);
                    break;
                case FilterProductState.NotActive:
                    query = query.Where(s => !s.IsActive && s.ProductAcceptanceState == ProductAcceptanceState.Accepted);
                    break;
                case FilterProductState.Accepted:
                    query = query.Where(s => s.ProductAcceptanceState == ProductAcceptanceState.Accepted);
                    break;
                case FilterProductState.Rejected:
                    query = query.Where(s => s.ProductAcceptanceState == ProductAcceptanceState.Rejected);
                    break;
                case FilterProductState.UnderProgress:
                    query = query.Where(s => s.ProductAcceptanceState == ProductAcceptanceState.UnderProgress);
                    break;
            }



            #endregion

            #region Filter

            if (!string.IsNullOrEmpty(filter.ProductTitle))
                query = query.Where(s => EF.Functions.Like(s.Title, $"%{filter.ProductTitle}%"));

            if (filter.StoreId != null && filter.StoreId != 0)
                query = query.Where(s => s.StoreId == filter.StoreId.Value);

            #endregion

            #region Paging

            var pager = Pager.Build(filter.PageId, await query.CountAsync(), filter.TakeEntity, filter.HomManyPageAfterAndBefore);
            var allEntities = await query.Paging(pager).ToListAsync();

            #endregion

            return filter.SetProducts(allEntities).SetPaging(pager);
        }

        public async Task<bool> AcceptSellerProduct(long productId)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product != null)
            {
                product.ProductAcceptanceState = ProductAcceptanceState.Accepted;
                product.ProductAcceptOrRejectDescription = $"محصول مورد نظر در تاریخ {DateTime.Now.ToShamsi()} مورد تایید سایت قرار گرفت";
                _productRepository.UpdateEntity(product);
                await _productRepository.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> RejectSellerProduct(RejectItemDTO reject)
        {
            var product = await _productRepository.GetByIdAsync(reject.Id);
            if (product != null)
            {
                product.ProductAcceptanceState = ProductAcceptanceState.Rejected;
                product.ProductAcceptOrRejectDescription = reject.RejectMessage;
                _productRepository.UpdateEntity(product);
                await _productRepository.SaveChangesAsync();

                return true;
            }

            return false;
        }
        public async Task<CreateProductResult> CreateProduct(CreateProductDTO product, long storeId, IFormFile productImage)
        {
            if (productImage == null) return CreateProductResult.HasNoImage;

            var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(productImage.FileName);

            var res = productImage.AddImageToServer(imageName, PathExtension.ProductImageImageServer, 150, 150, PathExtension.ProductThumbnailImageImageServer);

            if (res)
            {
                // create product
                var newProduct = new Product
                {
                    Title = product.Title,
                    Price = product.Price,
                    ShortDescription = product.ShortDescription,
                    Description = product.Description,
                    IsActive = product.IsActive,
                    StoreId = storeId,
                    ImageName = imageName,
                    ProductAcceptanceState = ProductAcceptanceState.UnderProgress
                };

                await _productRepository.AddEntityAsync(newProduct);
                await _productRepository.SaveChangesAsync();

                // create product categories
                var productSelectedCategories = new List<ProductSelectedCategory>();

                foreach (var categoryId in product.SelectedCategories)
                {
                    productSelectedCategories.Add(new ProductSelectedCategory
                    {
                        ProductCategoryId = categoryId,
                        ProductId = newProduct.Id
                    });
                }

                await _productSelectedCategoryRepository.AddRangeEntitiesAsync(productSelectedCategories);
                await _productSelectedCategoryRepository.SaveChangesAsync();

                // create product colors
                var productSelectedColors = new List<ProductColor>();

                foreach (var productColor in product.ProductColors)
                {
                    productSelectedColors.Add(new ProductColor
                    {
                        ColorName = productColor.ColorName,
                        Price = productColor.Price,
                        ProductId = newProduct.Id
                    });
                }

                await _productColorReporsitory.AddRangeEntitiesAsync(productSelectedColors);
                await _productSelectedCategoryRepository.SaveChangesAsync();

                return CreateProductResult.Success;
            }

            return CreateProductResult.Error;
        }

        #endregion

        #region ProductCategories

        public async Task<List<ProductCategory>> GetAllProductCategoriesByParentIdAsync(long? parentId)
        {
            if (parentId == null || parentId == 0)
            {
                return await _productCategoryRepository.GetQuery()
                    .AsQueryable()
                    .Where(s => !s.IsDeleted && s.IsActive)
                    .ToListAsync();
            }

            return await _productCategoryRepository.GetQuery()
                .AsQueryable()
                .Where(s => !s.IsDeleted && s.IsActive && s.ParentId == parentId)
                .ToListAsync();
        }

        public async Task<List<ProductCategory>> GetAllActiveProductCategoriesAsync()
        {
            return await _productCategoryRepository.GetQuery().AsQueryable()
                .Where(s => s.IsActive && !s.IsDeleted).ToListAsync();
        }

        #endregion

        #region dispose

        public async ValueTask DisposeAsync()
        {
            await _productCategoryRepository.DisposeAsync();
            await _productRepository.DisposeAsync();
            await _productSelectedCategoryRepository.DisposeAsync();
        }

        #endregion
    }
}
