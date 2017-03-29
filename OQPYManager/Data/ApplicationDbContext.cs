using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OQPYManager.Models;
using OQPYManager.Models.CoreModels;
using OQPYModels.Models;
using OQPYModels.Models.CoreModels;
using System.Linq;

namespace OQPYManager.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<BaseVenue> Venues { get; set; }
        public DbSet<BaseLocation> Locations { get; set; }
        public DbSet<BasePriceTag> PriceTags { get; set; }
        public DbSet<BaseReservation> Reservations { get; set; }
        public DbSet<BaseResource> Resources { get; set; }
        public DbSet<BaseReview> Reviews { get; set; }
        public DbSet<BaseTag> Tags { get; set; }
        public DbSet<BaseWorkHours> VenueWorkHours { get; set; }
        public DbSet<BaseOwner> Owners { get; set; }
        public DbSet<BaseEmployee> Employees { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<BaseVenue>().HasKey(v => v.Id);
            builder.Entity<BaseVenue>().HasMany(v => v.Resources).WithOne(r => r.Venue );
            builder.Entity<BaseVenue>().HasMany(v => v.Reviews).WithOne(r => r.Venue);
            builder.Entity<BaseVenue>().HasMany(v => v.PriceTags).WithOne(p => p.Venue);
            builder.Entity<BaseVenue>().HasMany(v => v.Employees).WithOne((e) => e.Venue);
            builder.Entity<BaseVenue>().HasOne(v => v.Owner).WithMany((e) => e.Venues);
            builder.Entity<BaseVenue>().HasOne(v => v.WorkHours)
                .WithOne(wh => wh.Venue)
                .HasForeignKey<BaseWorkHours>(wh => wh.VenueId);

            builder.Entity<BaseLocation>().HasKey(l => l.Id);

            builder.Entity<BasePriceTag>().HasKey(pt => pt.Id);

            builder.Entity<BaseReservation>().HasKey(r => r.Id);

            builder.Entity<BaseResource>().HasKey(r => r.Id);
            builder.Entity<BaseResource>().HasMany(r => r.Reservations).WithOne(r => r.Resource);

            builder.Entity<BaseReview>().HasKey(r => r.Id);

            builder.Entity<BaseTag>().HasKey(t => t.Id);

            builder.Entity<BaseWorkHours>().HasKey(wh => wh.Id);

            builder.Entity<BaseEmployee>().HasKey(be => be.Id);

            builder.Entity<BaseOwner>().HasKey(bo => bo.Id);
            builder.Entity<BaseWorkHours>().HasMany(wh => wh.WorkTimes).WithOne(wt => wt.WorkHours);
            

            //Many-to-many relationship between venues and tags
            builder.Entity<BaseVenueTag>().HasKey(vt => new { vt.VenueId, vt.TagId });
            builder.Entity<BaseVenueTag>().HasOne(vt => vt.Venue)
                .WithMany(v => v.VenueTags)
                .HasForeignKey(vt => vt.VenueId);
            builder.Entity<BaseVenueTag>().HasOne(vt => vt.Tag)
                .WithMany(t => t.VenueTags)
                .HasForeignKey(vt => vt.TagId);
        }
    }
}