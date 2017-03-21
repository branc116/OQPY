using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using OQPYManager.Models.CoreModels;

namespace OQPYManager.Data
{
    public class VenuesDbContext : DbContext
    {
        public DbSet<Venue> Venues { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<PriceTag> PriceTags { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<WorkHours> WorkHourses { get; set; }


        public VenuesDbContext(DbContextOptions<VenuesDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Venue>().HasKey(v => v.Id);
            builder.Entity<Venue>().HasMany(v => v.Resources).WithOne(r => r.Venue);
            builder.Entity<Venue>().HasMany(v => v.Reviews).WithOne(r => r.Venue);
            builder.Entity<Venue>().HasMany(v => v.PriceTags).WithOne(p => p.Venue);
            builder.Entity<Venue>().HasMany(v => v.Employees).WithOne();

            //Many-to-many relationship between venues and tags
            builder.Entity<VenueTag>().HasKey(vt => new {vt.VenueId, vt.TagId});
            builder.Entity<VenueTag>().HasOne(vt => vt.Venue)
                .WithMany(v => v.VenueTags)
                .HasForeignKey(vt => vt.VenueId);
            builder.Entity<VenueTag>().HasOne(vt => vt.Tag)
                .WithMany(t => t.VenueTags)
                .HasForeignKey(vt => vt.TagId);

        }
    }
}
