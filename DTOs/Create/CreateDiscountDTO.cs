using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Revenue_Recognition_System.DTOs.Create
{
    public class CreateDiscountDTO
    {
        [Required]
        public int SoftwareId { get; set; }

        [Required]
        [MaxLength(100)]
        public string DiscountName { get; set; } = null!;

        [Required]
        [Precision(4, 2)]
        public decimal DiscountValue { get; set; }

        [Required]
        public DateTime ActiveFrom { get; set; }

        [Required]
        public DateTime ActiveTo { get; set; }
    }
}
