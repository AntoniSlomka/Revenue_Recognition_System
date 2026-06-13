using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Revenue_Recognition_System.Entities;

namespace Revenue_Recognition_System.Data
{
    public class DatabaseContext(DbContextOptions opt) : IdentityDbContext<User>(opt)
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<IndividualCustomer> IndividualCustomers { get; set; }
        public DbSet<CompanyCustomer> CompanyCustomers { get; set; }
        public DbSet<Software> Softwares { get; set; }
        public DbSet<SoftwareVersion> SoftwareVersions { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Customer
            modelBuilder.Entity<Customer>().UseTptMappingStrategy();

            modelBuilder.Entity<Customer>(e =>
            {
                e.ToTable("Customers");
                e.HasKey(e => e.CustomerId);
                e.Property(e => e.Address).IsRequired();
                e.Property(e => e.Email).IsRequired();
                e.Property(e => e.Phone).IsRequired();
            });

            // IndividualCustomer
            modelBuilder.Entity<IndividualCustomer>(e =>
            {
                e.ToTable("IndividualCustomers");
                e.Property(c => c.Pesel).IsRequired().HasMaxLength(11);
                e.HasIndex(c => c.Pesel).IsUnique();
                e.Property(c => c.FirstName).IsRequired();
                e.Property(c => c.LastName).IsRequired();
            });

            // CompanyCustomer
            modelBuilder.Entity<CompanyCustomer>(e =>
            {
                e.ToTable("CompanyCustomers");
                e.Property(c => c.KrsNumber).IsRequired().HasMaxLength(10);
                e.HasIndex(c => c.KrsNumber).IsUnique();
                e.Property(c => c.CompanyName).IsRequired();
            });

            //Software
            modelBuilder.Entity<Software>(e =>
            {
                e.ToTable("Softwares");
                e.HasKey(s => s.SoftwareId);
                e.Property(s => s.Name).IsRequired().HasMaxLength(100);
                e.Property(s => s.Description).IsRequired();
                e.Property(s => s.OneYearPrice).IsRequired().HasPrecision(10, 2);
                e.HasOne(s => s.Category)
                    .WithMany(c => c.Softwares)
                    .HasForeignKey(s => s.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            //Category
            modelBuilder.Entity<Category>(e =>
            {
                e.ToTable("Categories");
                e.HasKey(c => c.CategoryId);
                e.Property(c => c.Name).IsRequired().HasMaxLength(100);
                e.Property(c => c.Description).IsRequired().HasMaxLength(200);
            });

            // SoftwareVersion
            modelBuilder.Entity<SoftwareVersion>(e =>
            {
                e.ToTable("SoftwareVersions");
                e.HasKey(s => s.VersionId);
                e.Property(s => s.VersionName).IsRequired().HasMaxLength(100);
                e.Property(s => s.ReleaseDate).IsRequired();
                e.Property(s => s.Description).IsRequired();
                e.HasOne(s => s.Software)
                    .WithMany(s => s.SoftwareVersions)
                    .HasForeignKey(s => s.SoftwareId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            //Discount
            modelBuilder.Entity<Discount>(e =>
            {
                e.ToTable("Discounts");
                e.HasKey(d => d.DiscountId);
                e.Property(d => d.DiscountName).IsRequired().HasMaxLength(100);
                e.Property(d => d.DiscountValue).IsRequired().HasPrecision(4, 2);
                e.Property(d => d.ActiveFrom).IsRequired();
                e.Property(d => d.ActiveTo).IsRequired();
                e.HasOne(d => d.Software)
                    .WithMany(s => s.Discounts)
                    .HasForeignKey(d => d.SoftwareId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            //Payment
            modelBuilder.Entity<Payment>(e =>
            {
                e.ToTable("Payments");
                e.HasKey(p => p.PaymentId);
                e.Property(p => p.PaymentMethod).IsRequired().HasMaxLength(200);
                e.Property(p => p.Value).IsRequired().HasPrecision(10, 2);
                e.HasOne(p => p.Contract)
                    .WithMany(c => c.Payments)
                    .HasForeignKey(p => p.ContractId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            //Contract
            modelBuilder.Entity<Contract>(e =>
            {
                e.ToTable("Contracts");
                e.HasKey(c => c.ContractId);
                e.Property(c => c.StartDate).IsRequired();
                e.Property(c => c.EndDate).IsRequired();
                e.Property(c => c.AdditionalSupportYears).IsRequired(false);
                e.Property(c => c.HasReturningDiscount).IsRequired();
                e.Property(c => c.Status).IsRequired().HasConversion<string>().HasMaxLength(50);
                e.Property(c => c.SignedAt).IsRequired(false);
                e.Property(c => c.FinalPrice).IsRequired().HasPrecision(10, 2);
                e.HasOne(c => c.Customer)
                    .WithMany(c => c.Contracts)
                    .HasForeignKey(p => p.CustomerId)
                    .OnDelete(DeleteBehavior.Restrict);
                e.HasOne(c => c.SoftwareVersion)
                    .WithMany(c => c.Contracts)
                    .HasForeignKey(p => p.SoftwareVersionId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            //Seeding
            modelBuilder.Entity<Category>().HasData(
                    new Category { CategoryId = 1, Name = "Graphic Design", Description = "Software used for creating different types of graphic media."},
                    new Category { CategoryId = 2, Name = "Accounting", Description = "Software used for managing accounting in a company."},
                    new Category { CategoryId = 3, Name = "3D Design", Description = "Software used for creating 3D models."},
                    new Category { CategoryId = 4, Name = "Audio Design", Description = "Software used for creating audio tracks."}
                );

            //Software
            modelBuilder.Entity<Software>().HasData(
                    new Software { SoftwareId = 1, Name = "PhotoMarket", Description = "PhotoMarket is a picture editing software. (All the funcionalities of Photoshop included)", CategoryId = 1, OneYearPrice = 650.0M},
                    new Software { SoftwareId = 2, Name = "Accountant3000", Description = "Accountant3000 is a super easy to use accounting software.", CategoryId = 2, OneYearPrice = 840.0M},
                    new Software { SoftwareId = 3, Name = "IllustrationMaker", Description = "IllustrationMaker allows to make beatiful illustrations in no time.", CategoryId = 1, OneYearPrice = 780.0M},
                    new Software { SoftwareId = 4, Name = "Blonder", Description = "Blonder is a versatile tool for 3D design.", CategoryId = 3, OneYearPrice = 960.0M}

                );

            //SoftwareVersion   
            modelBuilder.Entity<SoftwareVersion>().HasData(
                new SoftwareVersion { VersionId = 1, SoftwareId = 1, VersionName = "1.0.0", Description = "Initial Release", ReleaseDate = new DateTime(2020, 10, 5) },
                new SoftwareVersion { VersionId = 2, SoftwareId = 1, VersionName = "1.5.0", Description = "Added filters and layer support", ReleaseDate = new DateTime(2021, 3, 12) },
                new SoftwareVersion { VersionId = 3, SoftwareId = 1, VersionName = "2.0.0", Description = "Major UI overhaul and performance improvements", ReleaseDate = new DateTime(2022, 7, 20) },

                new SoftwareVersion { VersionId = 4, SoftwareId = 2, VersionName = "1.0.0", Description = "Initial Release", ReleaseDate = new DateTime(2019, 5, 1) },
                new SoftwareVersion { VersionId = 5, SoftwareId = 2, VersionName = "1.2.0", Description = "Added tax report generation", ReleaseDate = new DateTime(2020, 2, 14) },
                new SoftwareVersion { VersionId = 6, SoftwareId = 2, VersionName = "2.0.0", Description = "Cloud sync and multi-currency support", ReleaseDate = new DateTime(2023, 1, 9) },

                new SoftwareVersion { VersionId = 7, SoftwareId = 3, VersionName = "1.0.0", Description = "Initial Release", ReleaseDate = new DateTime(2021, 6, 15) },
                new SoftwareVersion { VersionId = 8, SoftwareId = 3, VersionName = "1.3.0", Description = "Added vector tools and brush library", ReleaseDate = new DateTime(2022, 4, 3) },
                new SoftwareVersion { VersionId = 9, SoftwareId = 3, VersionName = "2.0.0", Description = "AI-assisted drawing and new export formats", ReleaseDate = new DateTime(2023, 9, 18) },

                new SoftwareVersion { VersionId = 10, SoftwareId = 4, VersionName = "1.0.0", Description = "Initial Release", ReleaseDate = new DateTime(2018, 11, 22) },
                new SoftwareVersion { VersionId = 11, SoftwareId = 4, VersionName = "1.4.0", Description = "Added sculpting tools and material editor", ReleaseDate = new DateTime(2020, 8, 30) },
                new SoftwareVersion { VersionId = 12, SoftwareId = 4, VersionName = "3.0.0", Description = "Real-time rendering and physics simulation", ReleaseDate = new DateTime(2024, 2, 11) }
                );

            //Discount
            modelBuilder.Entity<Discount>().HasData(
                    new Discount { DiscountId = 1, SoftwareId = 1, DiscountName = "PhotoMarket summer discount", DiscountValue = 0.10M, ActiveFrom = new DateTime(2000, 7, 1), ActiveTo = new DateTime(2000, 8, 31)},
                    new Discount { DiscountId = 2, SoftwareId = 2, DiscountName = "Accountant3000 summer discount", DiscountValue = 0.05M, ActiveFrom = new DateTime(2000, 7, 1), ActiveTo = new DateTime(2000, 8, 31)},
                    new Discount { DiscountId = 3, SoftwareId = 3, DiscountName = "IllustrationMaker summer discount", DiscountValue = 0.10M, ActiveFrom = new DateTime(2000, 7, 1), ActiveTo = new DateTime(2000, 8, 31)},
                    new Discount { DiscountId = 4, SoftwareId = 3, DiscountName = "IllustrationMaker back to school discount", DiscountValue = 0.15M, ActiveFrom = new DateTime(2000, 8, 15), ActiveTo = new DateTime(2000, 9, 30)}
                );
        }
    }
}
