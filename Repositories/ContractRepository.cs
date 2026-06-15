using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Revenue_Recognition_System.Data;
using Revenue_Recognition_System.DTOs.Create;
using Revenue_Recognition_System.DTOs.Get;
using Revenue_Recognition_System.Entities;
using System.ComponentModel;
using System.Runtime.InteropServices;

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

        private async Task<GetContractDTO> MapContract(Contract contract)
        {
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
                Status = contract.Status.ToString(),
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
        public async Task<List<GetContractShortDTO>> GetAllContracts()
        {
            var contracts = await _context.Contracts
                .Include(c => c.SoftwareVersion)
                .Include(c => c.Payments)
                .Select(c => new GetContractShortDTO
                {
                    ContractId = c.ContractId,
                    SoftwareId = c.SoftwareVersion.SoftwareId,
                    HasReturningDiscount = c.HasReturningDiscount,
                    Status = c.Status,
                    SignedAt = c.SignedAt,
                    FinalPrice = c.FinalPrice,
                    Payments = c.Payments.Select(p => new GetPaymentDTO
                    {
                        PaymentId = p.PaymentId,
                        PaymentMethod = p.PaymentMethod,
                        Value = p.Value
                    }).ToList()
                }).ToListAsync();

            return contracts;
        }

        public async Task<List<GetContractShortDTO>> GetAllContractsBySoftwareId(int id)
        {
            var software = await _context.Softwares.Where(s => s.SoftwareId == id).FirstOrDefaultAsync();

            if (software == null) throw new KeyNotFoundException($"Software with id: {id} not found.");

            var contracts = await _context.Contracts
                .Where(c => c.SoftwareVersion.SoftwareId == id)
                .Include(c => c.SoftwareVersion)
                .Include(c => c.Payments)
                .Select(c => new GetContractShortDTO
                {
                    ContractId = c.ContractId,
                    SoftwareId = c.SoftwareVersion.SoftwareId,
                    HasReturningDiscount = c.HasReturningDiscount,
                    Status = c.Status,
                    SignedAt = c.SignedAt,
                    FinalPrice = c.FinalPrice,
                    Payments = c.Payments.Select(p => new GetPaymentDTO
                    {
                        PaymentId = p.PaymentId,
                        PaymentMethod = p.PaymentMethod,
                        Value = p.Value
                    }).ToList()
                }).ToListAsync();

            return contracts;
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

            var previousContract = await _context.Contracts
                .Where(c => (c.CustomerId == customer.CustomerId 
                    && c.SoftwareVersion.SoftwareId == softwareVersion.SoftwareId 
                    && (c.Status == Enums.ContractStatus.Created || c.Status == Enums.ContractStatus.Signed)))
                .FirstOrDefaultAsync();

            if (previousContract != null) throw new InvalidDataException("Customer already has an existing contract for this product.");

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
            await UpdateInactiveContracts();

            var contract = await _context.Contracts
                .Where(c => c.ContractId == id)
                .Include(c => c.SoftwareVersion)
                    .ThenInclude(s => s.Software)
                        .ThenInclude(s => s.Category)
                .Include(c => c.Payments)
                .FirstOrDefaultAsync();

            if (contract == null) throw new KeyNotFoundException($"Contract with id: {id} not found.");

            return await MapContract(contract);
        }

        private async Task UpdateInactiveContracts()
        {
            var createdContracts = await _context.Contracts.Where(c => c.Status == Enums.ContractStatus.Created).ToListAsync();

            var expiredContracts = createdContracts.Where(c => DateTime.UtcNow > c.EndDate).ToList();

            foreach (var contract in expiredContracts)
            {
                contract.Status = Enums.ContractStatus.Inactive;
            }

            await _context.SaveChangesAsync();
        }

        public async Task ProccessContractPayment(int id, CreatePaymentDTO request)
        {
            await UpdateInactiveContracts();

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var contract = await _context.Contracts.Where(c => c.ContractId == id)
                    .Include(c => c.Payments)
                    .FirstOrDefaultAsync();

                if (contract == null) throw new KeyNotFoundException($"Contract with id: {id} not found.");

                if (contract.Status.ToString() == "Inactive" || contract.Status.ToString() == "Signed") throw new InvalidDataException("Only contracts with status 'Created' accept payments.");

                if (DateTime.UtcNow < contract.StartDate || contract.EndDate < DateTime.UtcNow) throw new InvalidDataException("Contract can only accept payments in the specified time range.");

                var totalPayed = contract.Payments.Sum(p => p.Value);

                if (totalPayed + request.Value > contract.FinalPrice)
                {
                    throw new InvalidDataException($"The sum of all payments cannot exceede the final price of the contract.");
                }

                var payment = new Payment
                {
                    Contract = contract,
                    PaymentMethod = request.PaymentMethod,
                    Value = request.Value
                };

                await _context.Payments.AddAsync(payment);

                if (totalPayed +  payment.Value == contract.FinalPrice)
                {
                    contract.Status = Enums.ContractStatus.Signed;
                    contract.SignedAt = DateTime.UtcNow;
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }

        }

        public async Task DeleteContractById(int id)
        {
            var contract = await _context.Contracts.Where(c => c.ContractId == id).FirstOrDefaultAsync();

            if (contract == null) throw new KeyNotFoundException($"Contract with id: {id} not found.");

            _context.Contracts.Remove(contract);
            await _context.SaveChangesAsync();
        }
    }
}
