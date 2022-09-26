using DataLayer.DTOs.Paging;
using DataLayer.Entities.Products;
using System.Collections.Generic;

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

        #endregion
    }

    public enum FilterProductState
    {
        UnderProgress,
        Accepted,
        Rejected ,
        Active , 
        NotActive
    }
}
