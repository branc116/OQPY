using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using System;

namespace OQPYManager.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
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

                    b.Property<string>("EmployeeId");

                    b.Property<string>("OwnerId");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("OwnerId");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("EmployeeId");

                    b.Property<string>("OwnerId");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("OwnerId");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.Property<string>("EmployeeId");

                    b.Property<string>("OwnerId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("OwnerId");

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

            modelBuilder.Entity("OQPYModels.Models.CoreModels.Employee", b =>
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

            modelBuilder.Entity("OQPYModels.Models.CoreModels.FacebookUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("FacebookUsers");
                });

            modelBuilder.Entity("OQPYModels.Models.CoreModels.Location", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Adress");

                    b.Property<double>("Latitude");

                    b.Property<double>("Longditude");

                    b.HasKey("Id");

                    b.ToTable("Locations");
                });

            modelBuilder.Entity("OQPYModels.Models.CoreModels.Owner", b =>
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

            modelBuilder.Entity("OQPYModels.Models.CoreModels.PriceTag", b =>
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

            modelBuilder.Entity("OQPYModels.Models.CoreModels.Reservation", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("EndReservationTime");

                    b.Property<string>("FacebookUsersId");

                    b.Property<string>("ResourceId");

                    b.Property<string>("SecretCode");

                    b.Property<DateTime>("StartReservationTime");

                    b.Property<string>("VenueId");

                    b.HasKey("Id");

                    b.HasIndex("FacebookUsersId");

                    b.HasIndex("ResourceId");

                    b.HasIndex("VenueId");

                    b.ToTable("Reservations");
                });

            modelBuilder.Entity("OQPYModels.Models.CoreModels.Resource", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Category");

                    b.Property<bool>("IOTEnabled");

                    b.Property<bool>("OQPYed");

                    b.Property<string>("SecreteCode");

                    b.Property<string>("StuffName");

                    b.Property<string>("VenueId");

                    b.HasKey("Id");

                    b.HasIndex("VenueId");

                    b.ToTable("Resources");
                });

            modelBuilder.Entity("OQPYModels.Models.CoreModels.Review", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Comment");

                    b.Property<int>("Helpfulness");

                    b.Property<int>("Rating");

                    b.Property<string>("VenueId");

                    b.HasKey("Id");

                    b.HasIndex("VenueId");

                    b.ToTable("Reviews");
                });

            modelBuilder.Entity("OQPYModels.Models.CoreModels.Tag", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("TagName");

                    b.Property<string>("VenueId");

                    b.HasKey("Id");

                    b.HasIndex("VenueId");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("OQPYModels.Models.CoreModels.Venue", b =>
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

            modelBuilder.Entity("OQPYModels.Models.CoreModels.VenueTag", b =>
                {
                    b.Property<string>("VenueId");

                    b.Property<string>("TagId");

                    b.HasKey("VenueId", "TagId");

                    b.HasIndex("TagId");

                    b.ToTable("VenueTag");
                });

            modelBuilder.Entity("OQPYModels.Models.CoreModels.WorkHours", b =>
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

            modelBuilder.Entity("OQPYModels.Models.CoreModels.WorkTime", b =>
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

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Claims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("OQPYModels.Models.CoreModels.Employee")
                        .WithMany("Claims")
                        .HasForeignKey("EmployeeId");

                    b.HasOne("OQPYModels.Models.CoreModels.Owner")
                        .WithMany("Claims")
                        .HasForeignKey("OwnerId");

                    b.HasOne("OQPYManager.Models.ApplicationUser")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("OQPYModels.Models.CoreModels.Employee")
                        .WithMany("Logins")
                        .HasForeignKey("EmployeeId");

                    b.HasOne("OQPYModels.Models.CoreModels.Owner")
                        .WithMany("Logins")
                        .HasForeignKey("OwnerId");

                    b.HasOne("OQPYManager.Models.ApplicationUser")
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.HasOne("OQPYModels.Models.CoreModels.Employee")
                        .WithMany("Roles")
                        .HasForeignKey("EmployeeId");

                    b.HasOne("OQPYModels.Models.CoreModels.Owner")
                        .WithMany("Roles")
                        .HasForeignKey("OwnerId");

                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("OQPYManager.Models.ApplicationUser")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("OQPYModels.Models.CoreModels.Employee", b =>
                {
                    b.HasOne("OQPYModels.Models.CoreModels.Venue", "Venue")
                        .WithMany("Employees")
                        .HasForeignKey("VenueId");
                });

            modelBuilder.Entity("OQPYModels.Models.CoreModels.PriceTag", b =>
                {
                    b.HasOne("OQPYModels.Models.CoreModels.Venue", "Venue")
                        .WithMany("PriceTags")
                        .HasForeignKey("VenueId");
                });

            modelBuilder.Entity("OQPYModels.Models.CoreModels.Reservation", b =>
                {
                    b.HasOne("OQPYModels.Models.CoreModels.FacebookUser", "FacebookUsers")
                        .WithMany("Reservations")
                        .HasForeignKey("FacebookUsersId");

                    b.HasOne("OQPYModels.Models.CoreModels.Resource", "Resource")
                        .WithMany("Reservations")
                        .HasForeignKey("ResourceId");

                    b.HasOne("OQPYModels.Models.CoreModels.Venue")
                        .WithMany("Reservations")
                        .HasForeignKey("VenueId");
                });

            modelBuilder.Entity("OQPYModels.Models.CoreModels.Resource", b =>
                {
                    b.HasOne("OQPYModels.Models.CoreModels.Venue", "Venue")
                        .WithMany("Resources")
                        .HasForeignKey("VenueId");
                });

            modelBuilder.Entity("OQPYModels.Models.CoreModels.Review", b =>
                {
                    b.HasOne("OQPYModels.Models.CoreModels.Venue", "Venue")
                        .WithMany("Reviews")
                        .HasForeignKey("VenueId");
                });

            modelBuilder.Entity("OQPYModels.Models.CoreModels.Tag", b =>
                {
                    b.HasOne("OQPYModels.Models.CoreModels.Venue")
                        .WithMany("Tags")
                        .HasForeignKey("VenueId");
                });

            modelBuilder.Entity("OQPYModels.Models.CoreModels.Venue", b =>
                {
                    b.HasOne("OQPYModels.Models.CoreModels.Location", "Location")
                        .WithMany()
                        .HasForeignKey("LocationId");

                    b.HasOne("OQPYModels.Models.CoreModels.Owner", "Owner")
                        .WithMany("Venues")
                        .HasForeignKey("OwnerId");
                });

            modelBuilder.Entity("OQPYModels.Models.CoreModels.VenueTag", b =>
                {
                    b.HasOne("OQPYModels.Models.CoreModels.Tag", "Tag")
                        .WithMany("VenueTags")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("OQPYModels.Models.CoreModels.Venue", "Venue")
                        .WithMany("VenueTags")
                        .HasForeignKey("VenueId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("OQPYModels.Models.CoreModels.WorkHours", b =>
                {
                    b.HasOne("OQPYModels.Models.CoreModels.Venue", "Venue")
                        .WithOne("WorkHours")
                        .HasForeignKey("OQPYModels.Models.CoreModels.WorkHours", "VenueId");
                });

            modelBuilder.Entity("OQPYModels.Models.CoreModels.WorkTime", b =>
                {
                    b.HasOne("OQPYModels.Models.CoreModels.WorkHours", "WorkHours")
                        .WithMany("WorkTimes")
                        .HasForeignKey("WorkHoursId");
                });
        }
    }
}