using Azure.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update.Internal;
using Revenue_Recognition_System.Data;
using Revenue_Recognition_System.DTOs.Create;
using Revenue_Recognition_System.DTOs.Get;
using Revenue_Recognition_System.DTOs.Patch;
using Revenue_Recognition_System.Entities;

namespace Revenue_Recognition_System.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly DatabaseContext _context;

        public CustomerRepository(DatabaseContext databaseContext)
        {
            _context = databaseContext;
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

            return customer.Id;
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

            return customer.Id;
        }

        public async Task<IGetCustomerSimpleDTO> GetCustomerById(int id)
        {
            var individualCustomer = await _context.IndividualCustomers.Where(c => c.Id == id).FirstOrDefaultAsync();

            if (individualCustomer != null)
            {
                return new GetIndividualCustomerSimpleDTO
                {
                    Id = individualCustomer.Id,
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

            var companyCustomer = await _context.CompanyCustomers.Where(c => c.Id == id).FirstOrDefaultAsync();

            if (companyCustomer != null)
            {
                return new GetCompanyCustomerSimpleDTO
                {
                    Id = companyCustomer.Id,
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
            var customer = await _context.IndividualCustomers.Where(c => c.Id == id).FirstOrDefaultAsync();

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
            var customer = await _context.CompanyCustomers.Where(c => c.Id == id).FirstOrDefaultAsync();

            if (customer == null) throw new KeyNotFoundException($"Customer with id: {id} not found.");

            customer.CompanyName = request.CompanyName ?? customer.CompanyName;
            customer.Address = request.Address ?? customer.Address;
            customer.Email = request.Email ?? customer.Email;
            customer.Phone = request.Phone ?? customer.Phone;

            await _context.SaveChangesAsync();
        }
    }
}
