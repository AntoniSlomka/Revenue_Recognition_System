using Microsoft.EntityFrameworkCore;
using Revenue_Recognition_System.Entities;

namespace Revenue_Recognition_System.Data
{
    public class DatabaseContext(DbContextOptions opt) : DbContext(opt)
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<IndividualCustomer> IndividualCustomers { get; set; }
        public DbSet<CompanyCustomer> CompanyCustomers { get; set; }

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
        }
    }
}
