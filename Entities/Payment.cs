using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Revenue_Recognition_System.Entities
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }

        public Contract Contract { get; set; } = null!;

        [Required]
        public int ContractId { get; set; }

        public Customer Customer { get; set; } = null!;

        [Required]
        public int CustomerId { get; set; }

        [Required]
        [MaxLength(200)]
        public string PaymentMethod { get; set; } = null!;

        [Required]
        [Precision(10, 2)]
        public decimal Value { get; set; }

    }
}
