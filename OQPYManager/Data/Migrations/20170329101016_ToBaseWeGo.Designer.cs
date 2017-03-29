using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using OQPYManager.Data;

namespace OQPYManager.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20170329101016_ToBaseWeGo")]
    partial class ToBaseWeGo
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1")
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

                    b.Property<string>("BaseEmployeeId");

                    b.Property<string>("BaseOwnerId");

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("BaseEmployeeId");

                    b.HasIndex("BaseOwnerId");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("BaseEmployeeId");

                    b.Property<string>("BaseOwnerId");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("BaseEmployeeId");

                    b.HasIndex("BaseOwnerId");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.Property<string>("BaseEmployeeId");

                    b.Property<string>("BaseOwnerId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("BaseEmployeeId");

                    b.HasIndex("BaseOwnerId");

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
                });

            modelBuilder.Entity("OQPYModels.Models.BaseEmployee", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp");

                    b.Property<string>("Email");

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("Name");

                    b.Property<string>("NormalizedEmail");

                    b.Property<string>("NormalizedUserName");

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<string>("Surname");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName");

                    b.Property<string>("VenueId");

                    b.HasKey("Id");

                    b.HasIndex("VenueId");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("OQPYModels.Models.BaseOwner", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp");

                    b.Property<string>("Email");

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("Name");

                    b.Property<string>("NormalizedEmail");

                    b.Property<string>("NormalizedUserName");

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<string>("Surname");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName");

                    b.HasKey("Id");

                    b.ToTable("Owners");
                });

            modelBuilder.Entity("OQPYModels.Models.CoreModels.BaseLocation", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Adress");

                    b.Property<double>("Latitude");

                    b.Property<double>("Longditude");

                    b.HasKey("Id");

                    b.ToTable("Locations");
                });

            modelBuilder.Entity("OQPYModels.Models.CoreModels.BasePriceTag", b =>
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

            modelBuilder.Entity("OQPYModels.Models.CoreModels.BaseReservation", b =>
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

            modelBuilder.Entity("OQPYModels.Models.CoreModels.BaseResource", b =>
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

            modelBuilder.Entity("OQPYModels.Models.CoreModels.BaseReview", b =>
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

            modelBuilder.Entity("OQPYModels.Models.CoreModels.BaseTag", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("BaseVenueId");

                    b.Property<string>("TagName");

                    b.HasKey("Id");

                    b.HasIndex("BaseVenueId");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("OQPYModels.Models.CoreModels.BaseVenue", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("ImageUrl");

                    b.Property<string>("LocationId");

                    b.Property<string>("Name");

                    b.Property<string>("OwnerId");

                    b.Property<DateTime>("VenueCreationDate");

                    b.HasKey("Id");

                    b.HasIndex("LocationId");

                    b.HasIndex("OwnerId");

                    b.ToTable("Venues");
                });

            modelBuilder.Entity("OQPYModels.Models.CoreModels.BaseVenueTag", b =>
                {
                    b.Property<string>("VenueId");

                    b.Property<string>("TagId");

                    b.HasKey("VenueId", "TagId");

                    b.HasIndex("TagId");

                    b.ToTable("BaseVenueTag");
                });

            modelBuilder.Entity("OQPYModels.Models.CoreModels.BaseWorkHours", b =>
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

            modelBuilder.Entity("OQPYModels.Models.CoreModels.BaseWorkTime", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("EndTime");

                    b.Property<DateTime>("StartTime");

                    b.Property<string>("WorkHoursId");

                    b.HasKey("Id");

                    b.HasIndex("WorkHoursId");

                    b.ToTable("BaseWorkTime");
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
                    b.HasOne("OQPYModels.Models.BaseEmployee")
                        .WithMany("Claims")
                        .HasForeignKey("BaseEmployeeId");

                    b.HasOne("OQPYModels.Models.BaseOwner")
                        .WithMany("Claims")
                        .HasForeignKey("BaseOwnerId");

                    b.HasOne("OQPYManager.Models.ApplicationUser")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("OQPYModels.Models.BaseEmployee")
                        .WithMany("Logins")
                        .HasForeignKey("BaseEmployeeId");

                    b.HasOne("OQPYModels.Models.BaseOwner")
                        .WithMany("Logins")
                        .HasForeignKey("BaseOwnerId");

                    b.HasOne("OQPYManager.Models.ApplicationUser")
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.HasOne("OQPYModels.Models.BaseEmployee")
                        .WithMany("Roles")
                        .HasForeignKey("BaseEmployeeId");

                    b.HasOne("OQPYModels.Models.BaseOwner")
                        .WithMany("Roles")
                        .HasForeignKey("BaseOwnerId");

                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("OQPYManager.Models.ApplicationUser")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("OQPYModels.Models.BaseEmployee", b =>
                {
                    b.HasOne("OQPYModels.Models.CoreModels.BaseVenue", "Venue")
                        .WithMany("Employees")
                        .HasForeignKey("VenueId");
                });

            modelBuilder.Entity("OQPYModels.Models.CoreModels.BasePriceTag", b =>
                {
                    b.HasOne("OQPYModels.Models.CoreModels.BaseVenue", "Venue")
                        .WithMany("PriceTags")
                        .HasForeignKey("VenueId");
                });

            modelBuilder.Entity("OQPYModels.Models.CoreModels.BaseReservation", b =>
                {
                    b.HasOne("OQPYModels.Models.CoreModels.BaseResource", "Resource")
                        .WithMany("Reservations")
                        .HasForeignKey("ResourceId");
                });

            modelBuilder.Entity("OQPYModels.Models.CoreModels.BaseResource", b =>
                {
                    b.HasOne("OQPYModels.Models.CoreModels.BaseVenue", "Venue")
                        .WithMany("Resources")
                        .HasForeignKey("VenueId");
                });

            modelBuilder.Entity("OQPYModels.Models.CoreModels.BaseReview", b =>
                {
                    b.HasOne("OQPYModels.Models.CoreModels.BaseVenue", "Venue")
                        .WithMany("Reviews")
                        .HasForeignKey("VenueId");
                });

            modelBuilder.Entity("OQPYModels.Models.CoreModels.BaseTag", b =>
                {
                    b.HasOne("OQPYModels.Models.CoreModels.BaseVenue")
                        .WithMany("Tags")
                        .HasForeignKey("BaseVenueId");
                });

            modelBuilder.Entity("OQPYModels.Models.CoreModels.BaseVenue", b =>
                {
                    b.HasOne("OQPYModels.Models.CoreModels.BaseLocation", "Location")
                        .WithMany()
                        .HasForeignKey("LocationId");

                    b.HasOne("OQPYModels.Models.BaseOwner", "Owner")
                        .WithMany("Venues")
                        .HasForeignKey("OwnerId");
                });

            modelBuilder.Entity("OQPYModels.Models.CoreModels.BaseVenueTag", b =>
                {
                    b.HasOne("OQPYModels.Models.CoreModels.BaseTag", "Tag")
                        .WithMany("VenueTags")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("OQPYModels.Models.CoreModels.BaseVenue", "Venue")
                        .WithMany("VenueTags")
                        .HasForeignKey("VenueId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("OQPYModels.Models.CoreModels.BaseWorkHours", b =>
                {
                    b.HasOne("OQPYModels.Models.CoreModels.BaseVenue", "Venue")
                        .WithOne("WorkHours")
                        .HasForeignKey("OQPYModels.Models.CoreModels.BaseWorkHours", "VenueId");
                });

            modelBuilder.Entity("OQPYModels.Models.CoreModels.BaseWorkTime", b =>
                {
                    b.HasOne("OQPYModels.Models.CoreModels.BaseWorkHours", "WorkHours")
                        .WithMany("WorkTimes")
                        .HasForeignKey("WorkHoursId");
                });
        }
    }
}
