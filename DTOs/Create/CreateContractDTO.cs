using Revenue_Recognition_System.Entities;
using Revenue_Recognition_System.Enums;
using System.ComponentModel.DataAnnotations;

namespace Revenue_Recognition_System.DTOs.Create
{
    public class CreateContractDTO
    {
        [Required]
        public int CustomerId { get; set; }
        [Required]
        public int SoftwareVersionId { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        public int? AdditionalSupportYears { get; set; }
    }
}
