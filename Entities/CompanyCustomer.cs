using System.ComponentModel.DataAnnotations;

namespace Revenue_Recognition_System.Entities
{
    public class CompanyCustomer : Customer
    {
        [Required]
        [Length(minimumLength: 10, maximumLength: 10)]
        public string KrsNumber { get; private set; } = null!;

        [Required]
        [MaxLength(200)]
        public string CompanyName { get; set; } = null!;


    }
}
