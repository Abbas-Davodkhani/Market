using DataLayer.DTOs.Paging;
using DataLayer.Entities.Products;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataLayer.DTOs.Products
{
    public class ProductFilterDTO : BasePaging
    {
        #region Properties
        public string ProductTitle { get; set; }

        public long? StoreId { get; set; }

        public FilterProductState FilterProductState { get; set; }

        public List<Product> Products { get; set; }
        #endregion
        #region Methods
        public ProductFilterDTO SetProducts(List<Product> Products)
        {
            this.Products = Products;
            return this;
        }

        public ProductFilterDTO SetPaging(BasePaging paging)
        {
            this.PageId = paging.PageId;
            this.AllEntitiesCount = paging.AllEntitiesCount;
            this.StartPage = paging.StartPage;
            this.EndPage = paging.EndPage;
            this.HomManyPageAfterAndBefore = paging.HomManyPageAfterAndBefore;
            this.TakeEntity = paging.TakeEntity;
            this.SkipEntity = paging.SkipEntity;
            this.PageCount = paging.PageCount;
            return this;
        }

        public ProductFilterDTO GetCurrentPaging()
        {
            return this;
        }
        #endregion
    }

    public enum FilterProductState
    {
        [Display(Name = "همه")]
        All,
        [Display(Name = "در حال بررسی")]
        UnderProgress,
        [Display(Name = "تایید شده")]
        Accepted,
        [Display(Name = "رد شده")]
        Rejected,
        [Display(Name = "فعال")]
        Active,
        [Display(Name = "غیر فعال")]
        NotActive
    }
}
