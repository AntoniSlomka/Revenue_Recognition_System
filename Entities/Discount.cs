using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Revenue_Recognition_System.Entities
{
    public class Discount
    {
        [Key]
        public int DiscountId { get; set; }

        public Software Software { get; set; } = null!;

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

        public List<Contract> Contracts { get; set; } = new();
    }
}
