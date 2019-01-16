using Microsoft.EntityFrameworkCore;

namespace SmartKitchenApi
{
    public class ApplicationDbContext: DbContext
    {

        #region Public Properties

        /// <summary>
        /// The settings for the application
        /// </summary>
        public DbSet<SmartKitchenModel> KitchenUpdates { get; set; }

        #endregion


        /// <summary>
        /// Default constructor expecting database options to be passed in
        /// </summary>
        /// <param name ="options"> the database context option

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {
      
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
