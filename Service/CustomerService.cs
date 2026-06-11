using Revenue_Recognition_System.DTOs;
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
    }
}
