using System.ComponentModel.DataAnnotations;

namespace Revenue_Recognition_System.DTOs.Create
{
    public class CreateUserDTO
    {
        [Required]
        public string UserName { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;
    }
}
