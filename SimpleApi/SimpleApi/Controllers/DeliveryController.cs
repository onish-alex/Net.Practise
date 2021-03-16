using System.Threading.Tasks;
using App.Core.Entity;
using App.DataAccess.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SimpleApi.DataTransfer.DeliveriesDto;

namespace SimpleApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryController : ControllerBase
    {
        private IRepository<Delivery> deliveryRepository;
        private IMapper mapper;

        public DeliveryController(
            IRepository<Delivery> deliveryRepository,
            IMapper mapper)
        {
            this.deliveryRepository = deliveryRepository;
            this.mapper = mapper;
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(Delivery), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Get(int id)
        {
            var delivery = await this.deliveryRepository.GetByIdWithIncludesAsync(id, null, false);

            if (delivery == null)
            {
                this.ModelState.AddModelError("DeliveryIsNotExist", "Запрашиваемой доставки не существует");
                return this.NotFound(this.ModelState["DeliveryIsNotExist"]);
            }

            return new ObjectResult(delivery);
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Post(DeliveryCreateDto deliveryCreateDto)
        {
            var deliveryEntity = this.mapper.Map<Delivery>(deliveryCreateDto);
            await this.deliveryRepository.CreateAsync(deliveryEntity);
            return this.Created($"/api/Customer/{deliveryEntity.Id}", null);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Put(int id, DeliveryDto deliveryDto)
        {
            if (deliveryDto.Id != id)
            {
                this.ModelState.AddModelError("DeliveryWrongId", "Неверно указанный идентификатор");
                return this.BadRequest(this.ModelState);
            }

            var customerEntity = this.mapper.Map<Delivery>(deliveryDto);
            var trackedEntity = await this.deliveryRepository.GetByIdWithIncludesAsync(customerEntity.Id);

            if (trackedEntity == null)
            {
                this.ModelState.AddModelError("DeliveryIsNotExist", "Запрашиваемой доставки не существует");
                return this.BadRequest(this.ModelState);
            }

            trackedEntity.Address = customerEntity.Address;
            trackedEntity.TypeDelivery = customerEntity.TypeDelivery;

            await this.deliveryRepository.UpdateAsync(trackedEntity);

            return this.NoContent();
        }
    }
}
