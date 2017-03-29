using Microsoft.EntityFrameworkCore;

namespace OQPYManager.Data
{
    public class VenuesDbContext : DbContext
    {
        //everything was moved into applivcationDbContext
        public VenuesDbContext(DbContextOptions<VenuesDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
        }
    }
}
