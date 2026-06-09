using Microsoft.EntityFrameworkCore;
using Revenue_Recognition_System.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace Revenue_Recognition_System.Entities
{
    public class Software
    {
        [Key]
        public int SoftwareId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        [Required]
        public string Description { get; set; } = null!;

        [Required]
        [Precision(10, 2)]
        public decimal OneYearPrice { get; set; }

        public Category Category { get; set; } = null!;

        [Required]
        public int CategoryId { get; set; }

        public List<SoftwareVersion> SoftwareVersions { get; set; } = new();

        public List<Discount> Discounts { get; set; } = new();
    }
}
