using Revenue_Recognition_System.DTOs.Create;
using Revenue_Recognition_System.DTOs.Get;

namespace Revenue_Recognition_System.Repositories.Discounts
{
    public interface IDiscountRepository
    {
        Task<int> AddDiscount(CreateDiscountDTO request);
        Task<GetDiscountDTO> GetDiscountById(int id);
    }
}
