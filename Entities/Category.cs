using System.ComponentModel.DataAnnotations;

namespace Revenue_Recognition_System.Entities
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(200)]
        public string Description { get; set; } = null!;

        public List<Software> Softwares { get; set; } = new();
    }
}
