using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using OQPYManager.Data;

namespace OQPYManager.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20170328221344_InitalCreate")]
    partial class InitalCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("OQPYManager.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("Name");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<string>("Surname");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");

                    b.HasDiscriminator<string>("Discriminator").HasValue("ApplicationUser");
                });

            modelBuilder.Entity("OQPYManager.Models.CoreModels.Location", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Adress");

                    b.Property<double>("Latitude");

                    b.Property<double>("Longditude");

                    b.HasKey("Id");

                    b.ToTable("Locations");
                });

            modelBuilder.Entity("OQPYManager.Models.CoreModels.PriceTag", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ItemName");

                    b.Property<decimal>("Price");

                    b.Property<string>("VenueId");

                    b.HasKey("Id");

                    b.HasIndex("VenueId");

                    b.ToTable("PriceTags");
                });

            modelBuilder.Entity("OQPYManager.Models.CoreModels.Reservation", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("EndReservationTime");

                    b.Property<string>("ResourceId");

                    b.Property<string>("SecretCode");

                    b.Property<DateTime>("StartReservationTime");

                    b.HasKey("Id");

                    b.HasIndex("ResourceId");

                    b.ToTable("Reservations");
                });

            modelBuilder.Entity("OQPYManager.Models.CoreModels.Resource", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Category");

                    b.Property<string>("StuffName");

                    b.Property<string>("VenueId");

                    b.HasKey("Id");

                    b.HasIndex("VenueId");

                    b.ToTable("Resources");
                });

            modelBuilder.Entity("OQPYManager.Models.CoreModels.Review", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Comment");

                    b.Property<int>("Rating");

                    b.Property<string>("VenueId");

                    b.HasKey("Id");

                    b.HasIndex("VenueId");

                    b.ToTable("Reviews");
                });

            modelBuilder.Entity("OQPYManager.Models.CoreModels.Tag", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("TagName");

                    b.Property<string>("VenueId");

                    b.HasKey("Id");

                    b.HasIndex("VenueId");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("OQPYManager.Models.CoreModels.Venue", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("LocationId");

                    b.Property<string>("Name");

                    b.Property<string>("OwnerId");

                    b.Property<DateTime>("VenueCreationDate");

                    b.HasKey("Id");

                    b.HasIndex("LocationId");

                    b.HasIndex("OwnerId");

                    b.ToTable("Venues");
                });

            modelBuilder.Entity("OQPYManager.Models.CoreModels.VenueTag", b =>
                {
                    b.Property<string>("VenueId");

                    b.Property<string>("TagId");

                    b.HasKey("VenueId", "TagId");

                    b.HasIndex("TagId");

                    b.ToTable("VenueTag");
                });

            modelBuilder.Entity("OQPYManager.Models.CoreModels.WorkHours", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("IsWorking");

                    b.Property<string>("VenueId");

                    b.HasKey("Id");

                    b.HasIndex("VenueId")
                        .IsUnique();

                    b.ToTable("VenueWorkHours");
                });

            modelBuilder.Entity("OQPYManager.Models.CoreModels.WorkTime", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("EndTime");

                    b.Property<DateTime>("StartTime");

                    b.Property<string>("WorkHoursId");

                    b.HasKey("Id");

                    b.HasIndex("WorkHoursId");

                    b.ToTable("WorkTime");
                });

            modelBuilder.Entity("OQPYManager.Models.Employee", b =>
                {
                    b.HasBaseType("OQPYManager.Models.ApplicationUser");

                    b.Property<string>("VenueId");

                    b.HasIndex("VenueId");

                    b.ToTable("Employee");

                    b.HasDiscriminator().HasValue("Employee");
                });

            modelBuilder.Entity("OQPYManager.Models.Owner", b =>
                {
                    b.HasBaseType("OQPYManager.Models.ApplicationUser");


                    b.ToTable("Owner");

                    b.HasDiscriminator().HasValue("Owner");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Claims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("OQPYManager.Models.ApplicationUser")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("OQPYManager.Models.ApplicationUser")
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("OQPYManager.Models.ApplicationUser")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("OQPYManager.Models.CoreModels.PriceTag", b =>
                {
                    b.HasOne("OQPYManager.Models.CoreModels.Venue", "Venue")
                        .WithMany("PriceTags")
                        .HasForeignKey("VenueId");
                });

            modelBuilder.Entity("OQPYManager.Models.CoreModels.Reservation", b =>
                {
                    b.HasOne("OQPYManager.Models.CoreModels.Resource", "Resource")
                        .WithMany("Reservations")
                        .HasForeignKey("ResourceId");
                });

            modelBuilder.Entity("OQPYManager.Models.CoreModels.Resource", b =>
                {
                    b.HasOne("OQPYManager.Models.CoreModels.Venue", "Venue")
                        .WithMany("Resources")
                        .HasForeignKey("VenueId");
                });

            modelBuilder.Entity("OQPYManager.Models.CoreModels.Review", b =>
                {
                    b.HasOne("OQPYManager.Models.CoreModels.Venue", "Venue")
                        .WithMany("Reviews")
                        .HasForeignKey("VenueId");
                });

            modelBuilder.Entity("OQPYManager.Models.CoreModels.Tag", b =>
                {
                    b.HasOne("OQPYManager.Models.CoreModels.Venue")
                        .WithMany("Tags")
                        .HasForeignKey("VenueId");
                });

            modelBuilder.Entity("OQPYManager.Models.CoreModels.Venue", b =>
                {
                    b.HasOne("OQPYManager.Models.CoreModels.Location", "Location")
                        .WithMany()
                        .HasForeignKey("LocationId");

                    b.HasOne("OQPYManager.Models.Owner", "Owner")
                        .WithMany("Venues")
                        .HasForeignKey("OwnerId");
                });

            modelBuilder.Entity("OQPYManager.Models.CoreModels.VenueTag", b =>
                {
                    b.HasOne("OQPYManager.Models.CoreModels.Tag", "Tag")
                        .WithMany("VenueTags")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("OQPYManager.Models.CoreModels.Venue", "Venue")
                        .WithMany("VenueTags")
                        .HasForeignKey("VenueId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("OQPYManager.Models.CoreModels.WorkHours", b =>
                {
                    b.HasOne("OQPYManager.Models.CoreModels.Venue", "Venue")
                        .WithOne("WorkHours")
                        .HasForeignKey("OQPYManager.Models.CoreModels.WorkHours", "VenueId");
                });

            modelBuilder.Entity("OQPYManager.Models.CoreModels.WorkTime", b =>
                {
                    b.HasOne("OQPYManager.Models.CoreModels.WorkHours", "WorkHours")
                        .WithMany("WorkTimes")
                        .HasForeignKey("WorkHoursId");
                });

            modelBuilder.Entity("OQPYManager.Models.Employee", b =>
                {
                    b.HasOne("OQPYManager.Models.CoreModels.Venue", "Venue")
                        .WithMany("Employees")
                        .HasForeignKey("VenueId");
                });
        }
    }
}
