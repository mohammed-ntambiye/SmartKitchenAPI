using Microsoft.EntityFrameworkCore;

namespace SmartKitchenApi
{
    public class ApplicationDbContext: DbContext
    {
        public DbSet<RestaurantOrdersModel> RestaurantOrders { get; set; }
        public DbSet<SmartKitchenModel> KitchenUpdates { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {
        
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
