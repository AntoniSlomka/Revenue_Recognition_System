using System.ComponentModel.DataAnnotations;

namespace Revenue_Recognition_System.DTOs.Patch
{
    public class PatchIndividualCustomerDTO
    {
        [MaxLength(50)]
        public string? FirstName { get; set; }

        [MaxLength(100)]
        public string? LastName { get; set; }
        public string? Address { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        public string? Phone { get; set; }
    }
}
