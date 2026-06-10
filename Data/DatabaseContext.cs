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
                e.HasKey(e => e.Id);
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
                e.HasOne(s => s.Category).WithMany(c => c.Softwares).HasForeignKey(s => s.CategoryId);
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
                e.HasOne(s => s.Software).WithMany(s => s.SoftwareVersions).HasForeignKey(s => s.SoftwareId);
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
                e.HasOne(d => d.Software).WithMany(s => s.Discounts).HasForeignKey(d => d.SoftwareId);
            });

            //Payment
            modelBuilder.Entity<Payment>(e =>
            {
                e.ToTable("Payments");
                e.HasKey(p => p.PaymentId);
                e.Property(p => p.PaymentMethod).IsRequired().HasMaxLength(200);
                e.Property(p => p.Value).IsRequired().HasPrecision(10, 2);
                e.HasOne(p => p.Customer).WithMany(c => c.Payments).HasForeignKey(p => p.CustomerId);
                e.HasOne(p => p.Contract).WithMany(c => c.Payments).HasForeignKey(p => p.ContractId);
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
                e.HasOne(c => c.Customer).WithMany(c => c.Contracts).HasForeignKey(p => p.CustomerId);
                e.HasOne(c => c.SoftwareVersion).WithMany(c => c.Contracts).HasForeignKey(p => p.SoftwareVersionId);
                e.HasOne(c => c.Discount).WithMany(c => c.Contracts).HasForeignKey(p => p.DiscountId).IsRequired(false);
            });
        }
    }
}
