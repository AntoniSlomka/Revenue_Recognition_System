using Revenue_Recognition_System.DTOs.Get;

namespace Revenue_Recognition_System.Services
{
    public interface IRevenueService
    {
        Task<GetProductRevenueDTO> GetProductRevenueById(int id, string? code);
    }
}
