using Revenue_Recognition_System.DTOs;

namespace Revenue_Recognition_System.Repository
{
    public interface ICustomerRepository
    {
        Task<int> AddIndividualCustomer(CreateIndividualCustomerDTO request);

        Task<int> AddCompanyCustomer(CreateCompanyCustomerDTO request);

        Task<IGetCustomerSimpleDTO> GetCustomerById(int id);
    }
}
