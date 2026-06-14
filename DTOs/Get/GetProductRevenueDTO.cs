using System.ComponentModel.DataAnnotations;

namespace Revenue_Recognition_System.DTOs.Get
{
    public class GetProductRevenueDTO
    {
        public int SoftwareId { get; set; }
        public string Name { get; set; } = null!;
        public decimal RevenueInPLN { get; set; }
        public string? AlternateCurrency { get; set; }
        public decimal? RevenueInAlternateCurrency { get; set; }
    }
}
