using System.ComponentModel.DataAnnotations;

namespace Revenue_Recognition_System.Entities
{
    public class IndividualCustomer : Customer
    {

        [Required]
        [Length(minimumLength: 11, maximumLength: 11)]
        public string Pesel { get; private set; } = null!;

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; } = null!;

        public bool IsDeleted { get; set; } = false;

        public DateTime? DeletedAt { get; set; } = null;
    }
}
