using Revenue_Recognition_System.DTOs.Create;
using Revenue_Recognition_System.DTOs.Get;
using Revenue_Recognition_System.DTOs.Patch;
using Revenue_Recognition_System.Repository;

namespace Revenue_Recognition_System.Service
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repository;

        public CustomerService(ICustomerRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> AddIndividualCustomer(CreateIndividualCustomerDTO request)
        {
            return await _repository.AddIndividualCustomer(request);
        }

        public async Task<int> AddCompanyCustomer(CreateCompanyCustomerDTO request)
        {
            return await _repository.AddCompanyCustomer(request);
        }

        public async Task<IGetCustomerSimpleDTO> GetCustomerById(int id)
        {
            return await _repository.GetCustomerById(id);
        }

        public async Task UpdateIndividualCustomer(int id, PatchIndividualCustomerDTO request)
        {
            await _repository.UpdateIndividualCustomer(id, request);
        }

        public async Task UpdateCompanyCustomer(int id, PatchCompanyCustomerDTO request)
        {
            await _repository.UpdateCompanyCustomer(id, request);
        }
    }
}
