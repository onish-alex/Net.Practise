using System.Threading.Tasks;
using App.Core.Entity;
using App.DataAccess.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SimpleApi.DataTransfer.CustomersDto;

namespace SimpleApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private IRepository<Customer> customerRepository;
        private IMapper mapper;

        public CustomerController(
            IRepository<Customer> customerRepository,
            IMapper mapper)
        {
            this.customerRepository = customerRepository;
            this.mapper = mapper;
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(Customer), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Get(int id)
        {
            var customer = await this.customerRepository.GetByIdWithIncludesAsync(id, null, false);

            if (customer == null)
            {
                this.ModelState.AddModelError("CustomerIsNotExist", "Запрашиваемого клиента не существует");
                return this.NotFound(this.ModelState["CustomerIsNotExist"]);
            }

            return new ObjectResult(customer);
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Post(CustomerCreateDto customerCreateDto)
        {
            var customerEntity = this.mapper.Map<Customer>(customerCreateDto);
            await this.customerRepository.CreateAsync(customerEntity);
            return this.Created($"/api/Customer/{customerEntity.Id}", null);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Put(int id, CustomerDto customerDto)
        {
            if (customerDto.Id != id)
            {
                this.ModelState.AddModelError("CustomerWrongId", "Неверно указанный идентификатор");
                return this.BadRequest(this.ModelState);
            }

            var customerEntity = this.mapper.Map<Customer>(customerDto);
            var trackedEntity = await this.customerRepository.GetByIdWithIncludesAsync(customerEntity.Id);

            if (trackedEntity == null)
            {
                this.ModelState.AddModelError("CustomerIsNotExist", "Запрашиваемого клиента не существует");
                return this.BadRequest(this.ModelState);
            }

            trackedEntity.FullName = customerEntity.FullName;
            trackedEntity.Email = customerEntity.Email;
            trackedEntity.PhoneNumber = customerEntity.PhoneNumber;

            await this.customerRepository.UpdateAsync(trackedEntity);

            return this.NoContent();
        }
    }
}
