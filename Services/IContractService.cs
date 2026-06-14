using Revenue_Recognition_System.DTOs.Create;
using Revenue_Recognition_System.DTOs.Get;

namespace Revenue_Recognition_System.Services
{
    public interface IContractService
    {
        Task<int> AddNewContract(CreateContractDTO request);

        Task<GetContractDTO> GetContractById(int id);

        Task ProccessContractPayment(int id, CreatePaymentDTO request);

        Task DeleteContractById(int id);
    }
}
