using System.ComponentModel.DataAnnotations;

namespace Revenue_Recognition_System.DTOs.Get
{
    public class GetCategoryDTO
    {
        public int CategoryId { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
    }
}
