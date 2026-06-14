using Microsoft.EntityFrameworkCore;
using Revenue_Recognition_System.Data;
using Revenue_Recognition_System.DTOs.Get;

namespace Revenue_Recognition_System.Repositories
{
    public class SoftwareRepository : ISoftwareRepository
    {
        private readonly DatabaseContext _context;

        public SoftwareRepository(DatabaseContext databaseContext)
        {
            _context = databaseContext;
        }

        public async Task<GetSoftwareDTO> GetSoftwareById(int id)
        {
            var software = await _context.Softwares
                .Where(s => s.SoftwareId == id)
                .Include(s => s.Category)
                .FirstOrDefaultAsync();

            if (software == null) throw new KeyNotFoundException($"Software with id: {id} not found.");

            return new GetSoftwareDTO
            {
                SoftwareId = software.SoftwareId,
                Name = software.Name,
                Description = software.Description,
                OneYearPrice = software.OneYearPrice,
                Category = new GetCategoryDTO
                {
                    CategoryId = software.CategoryId,
                    Name = software.Category.Name,
                    Description = software.Description
                }
            };
        }
    }
}
