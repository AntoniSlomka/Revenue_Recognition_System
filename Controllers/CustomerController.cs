using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Revenue_Recognition_System.DTOs;
using Revenue_Recognition_System.Service;

namespace Revenue_Recognition_System.Controllers
{
    [ApiController]
    [Route("api/customers")]
    [Authorize(Roles = "Employee, Admin")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpPost]
        [Route("add/individual")]
        public async Task<ActionResult> AddIndividualCustomer([FromBody] CreateIndividualCustomerDTO request)
        {
            var customerId = await _customerService.AddIndividualCustomer(request);
            var customer = await _customerService.GetCustomerById(customerId);

            return CreatedAtAction(nameof(GetCustomerById), new { id = customerId }, customer);
        }

        [HttpPost]
        [Route("add/company")]
        public async Task<ActionResult> AddCompanyCustomer([FromBody] CreateCompanyCustomerDTO request)
        {
            var customerId = await _customerService.AddCompanyCustomer(request);
            var customer = await _customerService.GetCustomerById(customerId);

            return CreatedAtAction(nameof(GetCustomerById), new { id = customerId }, customer);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<IGetCustomerSimpleDTO>> GetCustomerById(int id)
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
    }
}
