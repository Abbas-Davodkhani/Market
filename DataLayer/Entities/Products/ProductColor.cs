using DataLayer.Entities.Common;
using System.ComponentModel.DataAnnotations;

namespace DataLayer.Entities.Products
{
    public class ProductColor : BaseEntity
    {
        #region Properties
        public long ProductId { get; set; }
        [Required]
        public string ColorName { get; set; }
        public int Price { get; set; }
        #endregion

        #region Relations
        public Product Product { get; set; }
        #endregion
    }
}
