using Microsoft.EntityFrameworkCore;
using SmartKitchenApi.Data;
using SmartKitchenApi.Models;

namespace SmartKitchenApi
{
    public class ApplicationDbContext: DbContext, IApplicationDBContext
    {
        public DbSet<RestaurantOrdersModel> RestaurantOrders { get; set; }
        public DbSet<SmartKitchenModel> KitchenUpdates { get; set; }
        public DbSet<MenuModel> Menu { get; set; }
        public DbSet<BasketData> Basket { get; set; }
        public DbSet<CustomiseData> Customise { get; set; }
        public DbSet<ConfirmedOrders> ConfirmedOrders { get; set; }
        public DbSet<Stats> Stats { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {
        
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
        }
    } 
}
