using Azure.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update.Internal;
using Revenue_Recognition_System.Data;
using Revenue_Recognition_System.DTOs.Create;
using Revenue_Recognition_System.DTOs.Get;
using Revenue_Recognition_System.DTOs.Patch;
using Revenue_Recognition_System.Entities;

namespace Revenue_Recognition_System.Repositories.Customers
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly DatabaseContext _context;

        public CustomerRepository(DatabaseContext databaseContext)
        {
            _context = databaseContext;
        }

        private async Task<IndividualCustomer?> FindIndividualCustomerById(int id)
        {
            return await _context.IndividualCustomers.Where(c => (c.CustomerId == id && !c.IsDeleted)).FirstOrDefaultAsync();
        }

        private async Task<CompanyCustomer?> FindCompanyCustomerById(int id)
        {
            return await _context.CompanyCustomers.Where(c => c.CustomerId == id).FirstOrDefaultAsync();
        }

        public async Task<int> AddIndividualCustomer(CreateIndividualCustomerDTO request)
        {
            var customer = new IndividualCustomer(request.Pesel, request.FirstName, request.LastName)
            {
                Address = request.Address,
                Email = request.Email,
                Phone = request.Phone
            };

            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();

            return customer.CustomerId;
        }

        public async Task<int> AddCompanyCustomer(CreateCompanyCustomerDTO request)
        {
            var customer = new CompanyCustomer(request.KrsNumber, request.CompanyName)
            {
                Address = request.Address,
                Email = request.Email,
                Phone = request.Phone
            };

            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();

            return customer.CustomerId;
        }

        public async Task<IGetCustomerShortDTO> GetCustomerById(int id)
        {
            var individualCustomer = await FindIndividualCustomerById(id);

            if (individualCustomer != null)
            {
                return new GetIndividualCustomerShortDTO
                {
                    Id = individualCustomer.CustomerId,
                    Address = individualCustomer.Address,
                    Email = individualCustomer.Email,
                    Phone = individualCustomer.Phone,
                    Pesel = individualCustomer.Pesel,
                    FirstName = individualCustomer.FirstName,
                    LastName = individualCustomer.LastName,
                    IsDeleted = individualCustomer.IsDeleted,
                    DeletedAt = individualCustomer.DeletedAt
                };
            }

            var companyCustomer = await FindCompanyCustomerById(id);

            if (companyCustomer != null)
            {
                return new GetCompanyCustomerShortDTO
                {
                    Id = companyCustomer.CustomerId,
                    Address = companyCustomer.Address,
                    Email = companyCustomer.Email,
                    Phone = companyCustomer.Phone,
                    KrsNumber = companyCustomer.KrsNumber,
                    CompanyName = companyCustomer.CompanyName
                };
            } 
            else
            {
                throw new KeyNotFoundException($"Customer with id: {id} not found.");
            }

        }

        public async Task UpdateIndividualCustomer(int id, PatchIndividualCustomerDTO request)
        {
            var customer = await FindIndividualCustomerById(id);

            if (customer == null) throw new KeyNotFoundException($"Customer with id: {id} not found.");

            customer.FirstName = request.FirstName ?? customer.FirstName;
            customer.LastName = request.LastName ?? customer.LastName;
            customer.Address = request.Address ?? customer.Address;
            customer.Email = request.Email ?? customer.Email;
            customer.Phone = request.Phone ?? customer.Phone;

            await _context.SaveChangesAsync();
        }

        public async Task UpdateCompanyCustomer(int id, PatchCompanyCustomerDTO request)
        {
            var customer = await FindCompanyCustomerById(id);

            if (customer == null) throw new KeyNotFoundException($"Customer with id: {id} not found.");

            customer.CompanyName = request.CompanyName ?? customer.CompanyName;
            customer.Address = request.Address ?? customer.Address;
            customer.Email = request.Email ?? customer.Email;
            customer.Phone = request.Phone ?? customer.Phone;

            await _context.SaveChangesAsync();
        }
        public async Task SoftDeleteIndividualCustomer(int id)
        {
            var customer = await FindIndividualCustomerById(id);

            if (customer == null) throw new KeyNotFoundException($"Customer with id: {id} not found.");

            customer.IsDeleted = true;
            customer.DeletedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }
    }
}
