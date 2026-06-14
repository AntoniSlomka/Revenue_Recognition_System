namespace Revenue_Recognition_System.DTOs.Get
{
    public class GetTotalRevenue
    {
        public decimal RevenueInPLN { get; set; }
        public string? AlternateCurrency { get; set; }
        public decimal? RevenueInAlternateCurrency { get; set; }
    }
}
