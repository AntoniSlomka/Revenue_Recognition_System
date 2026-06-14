using Revenue_Recognition_System.DTOs.Create;
using Revenue_Recognition_System.DTOs.Get;
using Revenue_Recognition_System.Repositories;

namespace Revenue_Recognition_System.Services
{
    public class ContractService : IContractService
    {
        private readonly IContractRepository _repository;

        public ContractService(IContractRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> AddNewContract(CreateContractDTO request)
        {
            return await _repository.AddNewContract(request);
        }

        public async Task DeleteContractById(int id)
        {
            await _repository.DeleteContractById(id);
        }

        public async Task<GetContractDTO> GetContractById(int id)
        {
            return await _repository.GetContractById(id);
        }

        public async Task ProccessContractPayment(int id, CreatePaymentDTO request)
        {
            await _repository.ProccessContractPayment(id, request);
        }


    }
}
