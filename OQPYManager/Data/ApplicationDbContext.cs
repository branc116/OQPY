using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OQPYManager.Models;
using OQPYModels.Models;
using OQPYModels.Models.CoreModels;

namespace OQPYManager.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Venue> Venues { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<PriceTag> PriceTags { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<WorkHours> VenueWorkHours { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Employee> Employees { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Venue>().HasKey(v => v.Id);
            builder.Entity<Venue>().HasMany(v => v.Resources).WithOne(r => r.Venue);
            builder.Entity<Venue>().HasMany(v => v.Reviews).WithOne(r => r.Venue);
            builder.Entity<Venue>().HasMany(v => v.PriceTags).WithOne(p => p.Venue);
            builder.Entity<Venue>().HasMany(v => v.Employees).WithOne((e) => e.Venue);
            builder.Entity<Venue>().HasOne(v => v.Owner).WithMany((e) => e.Venues);
            builder.Entity<Venue>().HasOne(v => v.WorkHours)
                .WithOne(wh => wh.Venue)
                .HasForeignKey<WorkHours>(wh => wh.VenueId);

            builder.Entity<Location>().HasKey(l => l.Id);

            builder.Entity<PriceTag>().HasKey(pt => pt.Id);

            builder.Entity<Reservation>().HasKey(r => r.Id);

            builder.Entity<Resource>().HasKey(r => r.Id);
            builder.Entity<Resource>().HasMany(r => r.Reservations).WithOne(r => r.Resource);

            builder.Entity<Review>().HasKey(r => r.Id);

            builder.Entity<Tag>().HasKey(t => t.Id);

            builder.Entity<WorkHours>().HasKey(wh => wh.Id);

            builder.Entity<Employee>().HasKey(be => be.Id);

            builder.Entity<Owner>().HasKey(bo => bo.Id);
            builder.Entity<WorkHours>().HasMany(wh => wh.WorkTimes).WithOne(wt => wt.WorkHours);

            //Many-to-many relationship between venues and tags
            builder.Entity<VenueTag>().HasKey(vt => new { vt.VenueId, vt.TagId });
            builder.Entity<VenueTag>().HasOne(vt => vt.Venue)
                .WithMany(v => v.VenueTags)
                .HasForeignKey(vt => vt.VenueId);
            builder.Entity<VenueTag>().HasOne(vt => vt.Tag)
                .WithMany(t => t.VenueTags)
                .HasForeignKey(vt => vt.TagId);
        }

        //public DbSet<OQPYManager.Models.CoreModels.Venue> Venue { get; set; }
    }
}