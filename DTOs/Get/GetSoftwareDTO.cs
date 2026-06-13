using Microsoft.EntityFrameworkCore;
using Revenue_Recognition_System.Entities;
using System.ComponentModel.DataAnnotations;

namespace Revenue_Recognition_System.DTOs.Get
{
    public class GetSoftwareDTO
    {
        public int SoftwareId { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal OneYearPrice { get; set; }
        public GetCategoryDTO Category { get; set; } = null!;

    }
}
