using System.ComponentModel.DataAnnotations;

namespace Revenue_Recognition_System.Entities
{
    public class Customer
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public string Address { get; set; } = null!;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
        [Required]
        public string Phone { get; set; } = null!;

        public List<Contract> Contracts { get; set; } = new();
        public List<Payment> Payments { get; set; } = new();


    }
}
