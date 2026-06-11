using System.ComponentModel.DataAnnotations;

namespace Revenue_Recognition_System.DTOs.Patch
{
    public class PatchCompanyCustomerDTO
    {
        [MaxLength(200)]
        public string? CompanyName { get; set; }
        public string? Address { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        public string? Phone { get; set; }
    }
}
