using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Revenue_Recognition_System.DTOs.Get
{
    public class GetPaymentDTO
    {
        public int PaymentId { get; set; }
        public string PaymentMethod { get; set; } = null!;
        public decimal Value { get; set; }
    }
}
