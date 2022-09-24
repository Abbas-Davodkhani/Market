using DataLayer.DTOs.Paging;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataLayer.DTOs
{
    public class FilterRequestStoreDTo : BasePaging
    {
        #region Properties
        public long? UserId { get; set; }
        public string StoreName { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public FilterStoreState FilterStoreState { get; set; }
        public List<Entities.Store> Stores { get; set; }
        #endregion
        #region Methods
        public FilterRequestStoreDTo SetStores(List<Entities.Store> stores)
        {
            this.Stores = stores;
            return this;
        }

        public FilterRequestStoreDTo SetPaging(BasePaging paging)
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

    public enum FilterStoreState
    {
        [Display(Name = "همه")]
        All,
        [Display(Name = "در حال بررسی")]
        UnderProgress,
        [Display(Name = "تایید شده")]
        Accepted,
        [Display(Name = "رد شده")]
        Rejected
    }
}
