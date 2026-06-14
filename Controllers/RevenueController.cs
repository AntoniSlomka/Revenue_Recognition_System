using Microsoft.AspNetCore.Mvc;
using Revenue_Recognition_System.DTOs.Get;
using Revenue_Recognition_System.Exceptions;
using Revenue_Recognition_System.Services;

namespace Revenue_Recognition_System.Controllers
{
    [ApiController]
    [Route("api/revenue")]
    //[Authorize(Roles = "Admin, Employee")]
    public class RevenueController : ControllerBase
    {
        private readonly IRevenueService _revenueService;

        public RevenueController(IRevenueService revenueService)
        {
            _revenueService = revenueService;
        }

        [HttpGet]
        [Route("/product/{id:int}")]
        public async Task<ActionResult<GetProductRevenueDTO>> GetProductRevenue(int id, [FromQuery] string? currencyCode)
        {
            try
            {
                return Ok(await _revenueService.GetProductRevenueById(id, currencyCode));
            }
            catch (ExternalServerExcpetion ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("404"))
                {
                    return NotFound(ex.Message);
                }
                else if (ex.Message.Contains("400"))
                {
                    return BadRequest(ex.Message);
                }
                else
                {
                    return StatusCode(500, ex.Message);
                }
            }
        }
    }
}
