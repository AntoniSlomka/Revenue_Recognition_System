using System.ComponentModel.DataAnnotations;

namespace Revenue_Recognition_System.DTOs.Create
{
    public class CreateCompanyCustomerDTO
    {
        [Required]
        [Length(minimumLength: 10, maximumLength: 10)]
        public string KrsNumber { get; set; } = null!;

        [Required]
        [MaxLength(200)]
        public string CompanyName { get; set; } = null!;
        [Required]
        public string Address { get; set; } = null!;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
        [Required]
        public string Phone { get; set; } = null!;
    }
}
