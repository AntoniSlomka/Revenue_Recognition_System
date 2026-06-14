using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Revenue_Recognition_System.DTOs.Create
{
    public class CreatePaymentDTO
    {
        public int ContractId { get; set; }

        [Required]
        [MaxLength(200)]
        public string PaymentMethod { get; set; } = null!;

        [Required]
        [Precision(10, 2)]
        public decimal Value { get; set; }
    }
}
