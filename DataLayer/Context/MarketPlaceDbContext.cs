using DataLayer.Entities.Account;
using DataLayer.Entities.Site;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Context
{
    public class MarketPlaceDbContext : DbContext
    {
        #region Constructor
        public MarketPlaceDbContext(DbContextOptions<MarketPlaceDbContext> options) : base(options){ }
        #endregion


        #region Account
        public DbSet<User> Users { get; set; }
        #endregion
        #region Site
        public DbSet<SiteSetting> SiteSettings { get; set; }
        #endregion
        #region On Model Creating
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(x => x.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Cascade;
            }
            base.OnModelCreating(modelBuilder);
        }
        #endregion 
    }
}
