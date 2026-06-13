using Revenue_Recognition_System.Entities;
using Revenue_Recognition_System.Enums;
using System.ComponentModel.DataAnnotations;

namespace Revenue_Recognition_System.DTOs.Get
{
    public class GetContractDTO
    {
        public int ContractId { get; set; }
        public object Customer { get; set; } = null!;
        public GetSoftwareDTO Software { get; set; } = null!;
        public GetSoftwareVersionDTO SoftwareVersion { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int? AdditionalSupportYears { get; set; }
        public bool HasReturningDiscount { get; set; }
        public ContractStatus Status { get; set; }
        public DateTime? SignedAt { get; set; }
        public decimal FinalPrice { get; set; }
        public List<GetPaymentDTO> Payments { get; set; } = new();
        public decimal TotalPayed => Payments.Sum(p => p.Value);
    }
}
