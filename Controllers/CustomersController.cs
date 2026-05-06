using InstallFlow.Core.Interfaces;
using InstallFlow.Data.DTO.Customers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace InstallFlow.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllCustomers()
        {
            var getAll = await _customerService.GetAllCustomersAsync();
            return Ok(getAll);

        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomer(int id)
        {
            var customer = await _customerService.GetCustomerAsync(id);


            return Ok(customer);

        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateCustomer(CreateCustomerDto dto)
        {

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var customer = await _customerService.CreateCustomerAsync(dto, userId);

            //return Created("", customer);
            //return Ok();


            return CreatedAtAction(
                nameof(GetCustomer),      // Action som bygger Location-URL
                new { id = customer.Id }, // Id i URL:en → /api/customers/5
                customer                  // Ska vara i body
            );
        }



        [Authorize]
        [HttpPatch("{id}")]

        public async Task<IActionResult> UpdateCustomer(UpdateCustomerDto dto, int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var isAdmin = User.IsInRole("Admin");

            var customer = await _customerService.UpdateCustomerAsync(dto, id, userId, isAdmin);



            return Ok(customer);

        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            await _customerService.DeleteCustomerAsync(id);

            return NoContent();
        }

    }
}
