using Revenue_Recognition_System.DTOs.Get;

namespace Revenue_Recognition_System.Services
{
    public interface IRevenueService
    {
        Task<GetProductRevenueDTO> GetProductRevenueById(int id, string? code);

        Task<GetTotalRevenue> GetTotalRevenue(string? code);

        Task<GetProductRevenueDTO> GetPredictedProductRevenueById(int id, string? code);

        Task<GetTotalRevenue> GetPredictedTotalRevenue(string? code);
    }
}
