using Revenue_Recognition_System.DTOs.Create;
using Revenue_Recognition_System.DTOs.Get;

namespace Revenue_Recognition_System.Repositories
{
    public interface IContractRepository
    {
        Task<int> AddNewContract(CreateContractDTO request);

        Task<GetContractDTO> GetContractById(int id);

        Task ProccessContractPayment(int id, CreatePaymentDTO request);

        Task DeleteContractById(int id);
    }
}
