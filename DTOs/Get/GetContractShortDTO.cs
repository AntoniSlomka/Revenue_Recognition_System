using Revenue_Recognition_System.Enums;

namespace Revenue_Recognition_System.DTOs.Get
{
    public class GetContractShortDTO
    {
        public int ContractId { get; set; }
        public int SoftwareId { get; set; }
        public bool HasReturningDiscount { get; set; }
        public ContractStatus Status { get; set; }
        public DateTime? SignedAt { get; set; }
        public decimal FinalPrice { get; set; }
        public List<GetPaymentDTO> Payments { get; set; } = new();
        public decimal TotalPayed => Payments.Sum(p => p.Value);
    }
}
