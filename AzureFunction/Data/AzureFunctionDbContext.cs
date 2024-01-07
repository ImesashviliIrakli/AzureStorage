using AzureFunction.Models;
using Microsoft.EntityFrameworkCore;

namespace AzureFunction.Data
{
    public class AzureFunctionDbContext : DbContext
    {
        public AzureFunctionDbContext(DbContextOptions<AzureFunctionDbContext> dbContextOptions) : base(dbContextOptions) { }
            
        public DbSet<SalesRequest> SalesRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SalesRequest>(entity =>
            {
                entity.HasKey(c => c.Id);
            });
        }
    }
}
