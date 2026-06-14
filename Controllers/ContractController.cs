using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Revenue_Recognition_System.DTOs.Create;
using Revenue_Recognition_System.DTOs.Get;
using Revenue_Recognition_System.Repositories;
using Revenue_Recognition_System.Services;

namespace Revenue_Recognition_System.Controllers
{
    [ApiController]
    [Route("api/contracts")]
    //[Authorize(Roles = "Admin, Employee")]
    public class ContractController : ControllerBase
    {
        private readonly IContractService _contractService;

        public ContractController(IContractService contractService)
        {
            _contractService = contractService;
        }

        [HttpPost]
        public async Task<ActionResult> AddNewContract([FromBody] CreateContractDTO request)
        {
            try
            {
                var contractId = await _contractService.AddNewContract(request);
                var contract = await _contractService.GetContractById(contractId);
                return CreatedAtAction(nameof(GetContractById), new { id = contractId }, contract);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidDataException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult<GetContractDTO>> GetContractById(int id)
        {
            try
            {
                return Ok(await _contractService.GetContractById(id));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        [Route("{id:int}/payments")]
        public async Task<ActionResult> AddPaymentForContract(int id, [FromBody] CreatePaymentDTO request)
        {
            try
            {
                await _contractService.ProccessContractPayment(id, request);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);                
            }
            catch (InvalidDataException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteContract(int id)
        {
            try
            {
                await _contractService.DeleteContractById(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
