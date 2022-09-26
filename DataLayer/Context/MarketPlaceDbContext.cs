using DataLayer.Entities;
using DataLayer.Entities.Account;
using DataLayer.Entities.Contacts;
using DataLayer.Entities.Products;
using DataLayer.Entities.Site;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DataLayer.Context
{
    public class MarketPlaceDbContext : DbContext
    {
        #region Constructor
        public MarketPlaceDbContext(DbContextOptions<MarketPlaceDbContext> options) : base(options) { }
        #endregion
        #region Account
        public DbSet<User> Users { get; set; }
        #endregion
        #region Site
        public DbSet<SiteSetting> SiteSettings { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<SiteBanner> SiteBanners { get; set; }
        #endregion
        #region ContactUs
        public DbSet<ContactUs> ContactUses { get; set; }
        #endregion
        #region Store
        public DbSet<Store> Stores { get; set; }
        #endregion
        #region products

        public DbSet<ProductCategory> ProductCategories { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<ProductSelectedCategory> ProductSelectedCategories { get; set; }

        #endregion
        #region On Model Creating
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(x => x.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
            base.OnModelCreating(modelBuilder);
        }
        #endregion 
    }
}
