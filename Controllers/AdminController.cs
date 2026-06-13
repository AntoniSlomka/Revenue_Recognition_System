using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Revenue_Recognition_System.DTOs.Create;
using Revenue_Recognition_System.Entities;

namespace Revenue_Recognition_System.Controllers
{
    [ApiController]
    [Route("api/admin")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly UserManager<User> _userManager;

        public AdminController(UserManager<User> userManaager)
        {
            _userManager = userManaager;
        }

        [HttpPost]
        public async Task<ActionResult> Register([FromBody] CreateUserDTO request)
        {
            var user = new User { UserName = request.UserName };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            await _userManager.AddToRoleAsync(user, "Employee");
            return NoContent();
        }
    }
}
