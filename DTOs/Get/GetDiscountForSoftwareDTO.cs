namespace Revenue_Recognition_System.DTOs.Get
{
    public class GetDiscountForSoftwareDTO
    {
        public int DiscountId { get; set; }
        public string DiscountName { get; set; } = null!;
        public decimal DiscountValue { get; set; }
        public DateTime ActiveFrom { get; set; }
        public DateTime ActiveTo { get; set; }
    }
}
