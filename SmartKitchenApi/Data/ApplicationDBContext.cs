using Microsoft.EntityFrameworkCore;
using SmartKitchenApi.Data;

namespace SmartKitchenApi
{
    public class ApplicationDbContext: DbContext, IApplicationDBContext
    {
        public DbSet<RestaurantOrdersModel> RestaurantOrders { get; set; }
        public DbSet<SmartKitchenModel> KitchenUpdates { get; set; }
        public DbSet<MenuModel> Menu { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {
        
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
        }
    } 
}
