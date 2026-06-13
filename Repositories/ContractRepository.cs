using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Revenue_Recognition_System.Data;
using Revenue_Recognition_System.DTOs.Create;
using Revenue_Recognition_System.DTOs.Get;
using Revenue_Recognition_System.Entities;
using System.ComponentModel;

namespace Revenue_Recognition_System.Repositories
{
    public class ContractRepository : IContractRepository
    {
        private readonly DatabaseContext _context;

        public ContractRepository(DatabaseContext databaseContext)
        {
            _context = databaseContext;
        }

        private bool IsDiscountActive(Discount discount, DateTime date)
        {
            var from = new DateTime(date.Year, discount.ActiveFrom.Month, discount.ActiveFrom.Day);
            var to = new DateTime(date.Year, discount.ActiveTo.Month, discount.ActiveTo.Day);

            return from <= date && date <= to;
        }

        private async Task<bool> IsCustomerReturning(int id)
        {
            return await _context.Contracts.Where(c => c.CustomerId == id && c.Status == Enums.ContractStatus.Signed).AnyAsync();
        }

        public async Task<int> AddNewContract(CreateContractDTO request)
        {
            var softwareVersion = await _context.SoftwareVersions
                .Where(s => s.VersionId == request.SoftwareVersionId)
                .Include(s => s.Software)
                .FirstOrDefaultAsync();

            if (softwareVersion == null) throw new KeyNotFoundException($"Software Version with id: {request.SoftwareVersionId} not found.");

            var customer = await _context.Customers
                .Where(c => c.CustomerId == request.CustomerId)
                .FirstOrDefaultAsync();

            if (customer == null) throw new KeyNotFoundException($"Customer with id: {request.CustomerId} not found.");

            var allDiscounts = await _context.Discounts
                .Where(d => d.SoftwareId == softwareVersion.SoftwareId)
                .ToListAsync();

            var discounts = allDiscounts
                .Where(d => IsDiscountActive(d, DateTime.UtcNow))
                .ToList();

            var priceMult = 1.0M;

            if (discounts.Any())
            {
                var maxDiscount = discounts.Max(d => d.DiscountValue);

                priceMult *= (1.0M - maxDiscount);
            }
            var isReturning = await IsCustomerReturning(request.CustomerId);
            if (isReturning)
            {
                priceMult *= 0.95M;
            }

            var timeRange = request.EndDate - request.StartDate;

            if (timeRange.TotalDays < 3 || timeRange.TotalDays > 30) throw new InvalidDataException("Contract time range has to be between 3 and 30 days.");

            if (request.StartDate < DateTime.UtcNow) throw new InvalidDataException("Contract time range has to be in the future.");

            var price = softwareVersion.Software.OneYearPrice;

            if (request.AdditionalSupportYears != null)
            {
                price += (decimal)(1000.0M * request.AdditionalSupportYears);
            }

            price *= priceMult;

            var conract = new Contract
            {
                Customer = customer,
                SoftwareVersion = softwareVersion,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                AdditionalSupportYears = request.AdditionalSupportYears,
                HasReturningDiscount = isReturning,
                Status = Enums.ContractStatus.Created,
                FinalPrice = price
            };

            await _context.Contracts.AddAsync(conract);
            await _context.SaveChangesAsync();

            return conract.ContractId;
        }

        public async Task<GetContractDTO> GetContractById(int id)
        {
            var contract = await _context.Contracts
                .Where(c => c.ContractId == id)
                .Include(c => c.SoftwareVersion)
                    .ThenInclude(s => s.Software)
                        .ThenInclude(s => s.Category)
                .Include(c => c.Payments)
                .FirstOrDefaultAsync();

            if (contract == null) throw new KeyNotFoundException($"Contract with id: {id} not found.");

            IGetCustomerShortDTO? customerDTO = null;

            var individualCustomer = await _context.IndividualCustomers.Where(c => c.CustomerId == contract.CustomerId).FirstOrDefaultAsync();

            if (individualCustomer != null)
            {
                customerDTO = new GetIndividualCustomerShortDTO
                {
                    CustomerId = individualCustomer.CustomerId,
                    Pesel = individualCustomer.Pesel,
                    FirstName = individualCustomer.FirstName,
                    LastName = individualCustomer.LastName,
                    Address = individualCustomer.Address,
                    Email = individualCustomer.Email,
                    Phone = individualCustomer.Phone,
                    IsDeleted = individualCustomer.IsDeleted,
                    DeletedAt = individualCustomer.DeletedAt
                };
            } 
            else
            {
                var companyCustomer = await _context.CompanyCustomers.Where(c => c.CustomerId == contract.CustomerId).FirstOrDefaultAsync();

                if (companyCustomer != null)
                {
                    customerDTO = new GetCompanyCustomerShortDTO
                    {
                        CustomerId = companyCustomer.CustomerId,
                        KrsNumber = companyCustomer.KrsNumber,
                        CompanyName = companyCustomer.CompanyName,
                        Address = companyCustomer.Address,
                        Email = companyCustomer.Email,
                        Phone = companyCustomer.Phone
                    };
                }
                else
                {
                    throw new KeyNotFoundException($"Customer with id: {contract.CustomerId} not found.");
                }

            }

            return new GetContractDTO
            {
                ContractId = contract.ContractId,
                Customer = customerDTO,
                Software = new GetSoftwareDTO
                {
                    SoftwareId = contract.SoftwareVersion.Software.SoftwareId,
                    Name = contract.SoftwareVersion.Software.Name,
                    Description = contract.SoftwareVersion.Software.Description,
                    OneYearPrice = contract.SoftwareVersion.Software.OneYearPrice,
                    Category = new GetCategoryDTO
                    {
                        CategoryId = contract.SoftwareVersion.Software.Category.CategoryId,
                        Name = contract.SoftwareVersion.Software.Category.Name,
                        Description = contract.SoftwareVersion.Software.Category.Description
                    }
                },
                SoftwareVersion = new GetSoftwareVersionDTO
                {
                    VersionId = contract.SoftwareVersion.VersionId,
                    VersionName = contract.SoftwareVersion.VersionName,
                    Description = contract.SoftwareVersion.Description,
                    ReleaseDate = contract.SoftwareVersion.ReleaseDate
                },
                StartDate = contract.StartDate,
                EndDate = contract.EndDate,
                AdditionalSupportYears = contract.AdditionalSupportYears,
                HasReturningDiscount = contract.HasReturningDiscount,
                Status = contract.Status,
                SignedAt = contract.SignedAt,
                FinalPrice = contract.FinalPrice,
                Payments = contract.Payments.Select(p => new GetPaymentDTO
                {
                    PaymentId = p.PaymentId,
                    PaymentMethod = p.PaymentMethod,
                    Value = p.Value
                }).ToList()
            };
        }
    }
}
