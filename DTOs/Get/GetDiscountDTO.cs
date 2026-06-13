using Microsoft.EntityFrameworkCore;
using Revenue_Recognition_System.Entities;
using System.ComponentModel.DataAnnotations;

namespace Revenue_Recognition_System.DTOs.Get
{
    public class GetDiscountDTO
    {
        public int DiscountId { get; set; }
        public string DiscountName { get; set; } = null!;
        public int SoftwareId { get; set; }
        public string SoftwareName { get; set; } = null!;
        public decimal DiscountValue { get; set; }
        public DateTime ActiveFrom { get; set; }
        public DateTime ActiveTo { get; set; }
    }
}
