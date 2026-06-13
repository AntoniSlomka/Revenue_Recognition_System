using Revenue_Recognition_System.Enums;
using System.ComponentModel.DataAnnotations;

namespace Revenue_Recognition_System.Entities
{
    public class Contract
    {
        [Key]
        public int ContractId { get; set; }

        public Customer Customer { get; set; } = null!;

        [Required]
        public int CustomerId { get; set; }

        public SoftwareVersion SoftwareVersion { get; set; } = null!;

        [Required]
        public int SoftwareVersionId { get; set; }

        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }

        public int? AdditionalSupportYears { get; set; }

        [Required]
        public bool HasReturningDiscount { get; set; }

        [Required]
        public ContractStatus Status { get; set; }
        
        public DateTime? SignedAt { get; set; }

        [Required]
        public decimal FinalPrice { get; set; }

        public List<Payment> Payments { get; set; } = new();
    }
}
