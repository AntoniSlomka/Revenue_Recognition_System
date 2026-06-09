using System.ComponentModel.DataAnnotations;

namespace Revenue_Recognition_System.Entities
{
    public class SoftwareVersion
    {
        [Key]
        public int VersionId { get; set; }
        
        public Software Software { get; set; } = null!;
        [Required]
        public int SoftwareId { get; set; }
        [Required]
        [MaxLength(100)]
        public string VersionName { get; set; } = null!;

        [Required]
        public string Description { get; set; } = null!;

        [Required]
        public DateTime ReleaseDate { get; set; }

        public List<Contract> Contracts { get; set; } = new();
    }
}
