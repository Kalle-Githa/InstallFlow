using InstallFlow.Core.Interfaces;
using InstallFlow.Data.DTO;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet]
        public async Task<IActionResult> GetAllCustomers()
        {
            var getAll = await _customerService.GetAllCustomersAsync();
            return Ok(getAll);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomer(int id)
        {
            var customer = await _customerService.GetCustomerAsync(id);
            if (customer == null)
            {
                return NotFound();

            }


            return Ok(customer);

        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer(CreateCustomerDto dto)
        {
            var customer = await _customerService.CreateCustomerAsync(dto);

            return CreatedAtAction(
                nameof(GetCustomer),      // Action som bygger Location-URL
                new { id = customer.Id }, // Id i URL:en → /api/customers/5
                customer                  // Ska vara i body
            );
        }







    }
}
