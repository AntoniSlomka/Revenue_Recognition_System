using Revenue_Recognition_System.DTOs.Create;
using Revenue_Recognition_System.DTOs.Get;
using Revenue_Recognition_System.DTOs.Patch;

namespace Revenue_Recognition_System.Service
{
    public interface ICustomerService
    {
        Task<int> AddIndividualCustomer(CreateIndividualCustomerDTO request);

        Task<int> AddCompanyCustomer(CreateCompanyCustomerDTO request);

        Task<IGetCustomerSimpleDTO> GetCustomerById(int id);

        Task UpdateIndividualCustomer(int id, PatchIndividualCustomerDTO request);

        Task UpdateCompanyCustomer(int id, PatchCompanyCustomerDTO request);

        Task SoftDeleteIndividualCustomer(int id);
    }
}
