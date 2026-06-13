using Microsoft.EntityFrameworkCore;
using Revenue_Recognition_System.Data;
using Revenue_Recognition_System.DTOs.Create;
using Revenue_Recognition_System.DTOs.Get;
using Revenue_Recognition_System.Entities;

namespace Revenue_Recognition_System.Repositories.Discounts
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly DatabaseContext _context;

        public DiscountRepository(DatabaseContext databaseContext)
        {
            _context = databaseContext;
        }

        public async Task<int> AddDiscount(CreateDiscountDTO request)
        {
            var software = await _context.Softwares.Where(s => s.SoftwareId == request.SoftwareId).FirstOrDefaultAsync();

            if (software == null) throw new KeyNotFoundException($"Software with id: {request.SoftwareId} not found.");

            var discount = new Discount
            {
                Software = software,
                DiscountName = request.DiscountName,
                DiscountValue = request.DiscountValue,
                ActiveFrom = request.ActiveFrom,
                ActiveTo = request.ActiveTo
            };

            await _context.Discounts.AddAsync(discount);
            await _context.SaveChangesAsync();

            return discount.DiscountId;
        }

        public async Task<GetDiscountDTO> GetDiscountById(int id)
        {
            var discount = await _context.Discounts
                .Where(d => d.DiscountId == id)
                .Include(d => d.Software)
                .FirstOrDefaultAsync();

            if (discount == null) throw new KeyNotFoundException($"Discount with id: {id} not found.");

            return new GetDiscountDTO
            {
                DiscountId = discount.DiscountId,
                DiscountName = discount.DiscountName,
                SoftwareId = discount.SoftwareId,
                SoftwareName = discount.Software.Name,
                DiscountValue = discount.DiscountValue,
                ActiveFrom = discount.ActiveFrom,
                ActiveTo = discount.ActiveTo
            };
        }
    }
}
