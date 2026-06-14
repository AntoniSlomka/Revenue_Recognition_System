using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Revenue_Recognition_System.DTOs.Create;
using Revenue_Recognition_System.DTOs.Get;
using Revenue_Recognition_System.DTOs.Patch;
using Revenue_Recognition_System.Services;

namespace Revenue_Recognition_System.Controllers
{
    [ApiController]
    [Route("api/customers")]
    [Authorize]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpPost]
        [Route("individual")]
        [Authorize(Roles = "Employee, Admin")]
        public async Task<ActionResult> AddIndividualCustomer([FromBody] CreateIndividualCustomerDTO request)
        {
            try
            {
                var customerId = await _customerService.AddIndividualCustomer(request);
                var customer = await _customerService.GetCustomerById(customerId);

                return CreatedAtAction(nameof(GetCustomerById), new { id = customerId }, customer);
            }
            catch (DbUpdateException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpPost]
        [Route("company")]
        [Authorize(Roles = "Employee, Admin")]
        public async Task<ActionResult> AddCompanyCustomer([FromBody] CreateCompanyCustomerDTO request)
        {
            try
            {
                var customerId = await _customerService.AddCompanyCustomer(request);
                var customer = await _customerService.GetCustomerById(customerId);

                return CreatedAtAction(nameof(GetCustomerById), new { id = customerId }, customer);
            }
            catch (DbUpdateException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Route("{id:int}")]
        [Authorize(Roles = "Employee, Admin")]
        public async Task<ActionResult<IGetCustomerShortDTO>> GetCustomerById(int id)
        {
            try
            {
                return Ok(await _customerService.GetCustomerById(id));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);                
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPatch]
        [Route("individual/{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateIndividualCustomer(int id, PatchIndividualCustomerDTO request)
        {
            try
            {
                await _customerService.UpdateIndividualCustomer(id, request);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPatch]
        [Route("company/{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateCompanyCustomer(int id, PatchCompanyCustomerDTO request)
        {
            try
            {
                await _customerService.UpdateCompanyCustomer(id, request);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete]
        [Route("individual/{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> SoftDeleteIndividualCustomer(int id)
        {
            try
            {
                await _customerService.SoftDeleteIndividualCustomer(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
