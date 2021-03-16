using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using App.Core.Entity;
using App.DataAccess.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SimpleApi.DataTransfer.ContractsDto;

namespace SimpleApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContractController : ControllerBase
    {
        private IRepository<Contract> contractRepository;
        private IRepository<Customer> customerRepository;
        private IRepository<Delivery> deliveryRepository;
        private IRepository<Item> itemRepository;
        private IMapper mapper;

        public ContractController(
            IRepository<Contract> contractRepository,
            IRepository<Customer> customerRepository,
            IRepository<Delivery> deliveryRepository,
            IRepository<Item> itemRepository,
            IMapper mapper)
        {
            this.contractRepository = contractRepository;
            this.deliveryRepository = deliveryRepository;
            this.customerRepository = customerRepository;
            this.itemRepository = itemRepository;
            this.mapper = mapper;
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(Contract), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Get(int id)
        {
            var contract = await this.contractRepository.GetByIdWithIncludesAsync(
                id,
                new List<Expression<Func<Contract, dynamic>>>()
                {
                    x => x.Customer,
                    x => x.Delivery,
                    x => x.Items,
                });

            if (contract == null)
            {
                this.ModelState.AddModelError("ContractIsNotExist", "Запрашиваемого контракта не существует");
                return this.NotFound(this.ModelState["ContractIsNotExist"]);
            }

            foreach (var item in contract.Items)
            {
                item.Contracts = null;
            }

            return new ObjectResult(contract);
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Post(ContractCreateUpdateDto contractCreateDto)
        {
            if (!await this.deliveryRepository.ExistAsync(x => x.Id == contractCreateDto.DeliveryId))
            {
                this.ModelState.AddModelError("ContractDeliveryIsNotExist", $"Доставки с идентификатором {contractCreateDto.DeliveryId} не существует");
                return this.BadRequest(this.ModelState);
            }

            if (!await this.customerRepository.ExistAsync(x => x.Id == contractCreateDto.CustomerId))
            {
                this.ModelState.AddModelError("ContractDeliveryIsNotExist", $"Клиента с идентификатором {contractCreateDto.CustomerId} не существует");
                return this.BadRequest(this.ModelState);
            }

            var itemsToSet = new List<Item>();

            foreach (var itemId in contractCreateDto.ItemId)
            {
                var item = await this.itemRepository.GetByIdWithIncludesAsync(itemId);

                if (item == null)
                {
                    this.ModelState.AddModelError("ContractItemIsNotExist", $"Товара с идентификатором {itemId} не существует");
                    return this.BadRequest(this.ModelState);
                }

                itemsToSet.Add(item);
            }

            var contractEntity = this.mapper.Map<Contract>(contractCreateDto);
            contractEntity.Items = new List<Item>();

            await this.contractRepository.CreateAsync(contractEntity);

            foreach (var item in itemsToSet)
            {
                contractEntity.Items.Add(item);
            }

            await this.contractRepository.UpdateAsync(contractEntity);

            return this.Created($"/api/Customer/{contractEntity.Id}", null);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Put(int id, ContractCreateUpdateDto contractDto)
        {
            var trackedEntity = await this.contractRepository.GetByIdWithIncludesAsync(
                id,
                new List<Expression<Func<Contract, dynamic>>>()
                {
                    x => x.Items,
                },
                false);

            if (trackedEntity == null)
            {
                this.ModelState.AddModelError("ContractIsNotExist", "Запрашиваемого контракта не существует");
                return this.BadRequest(this.ModelState);
            }

            if (!await this.deliveryRepository.ExistAsync(x => x.Id == contractDto.DeliveryId))
            {
                this.ModelState.AddModelError("ContractDeliveryIsNotExist", $"Доставки с идентификатором {contractDto.DeliveryId} не существует");
                return this.BadRequest(this.ModelState);
            }

            if (!await this.customerRepository.ExistAsync(x => x.Id == contractDto.CustomerId))
            {
                this.ModelState.AddModelError("ContractDeliveryIsNotExist", $"Клиента с идентификатором {contractDto.CustomerId} не существует");
                return this.BadRequest(this.ModelState);
            }

            var itemsToSet = new List<Item>();

            foreach (var itemId in contractDto.ItemId)
            {
                if (trackedEntity.Items.Any(x => x.Id == itemId))
                {
                    itemsToSet.Add(trackedEntity.Items.Single(x => x.Id == itemId));
                }
                else
                {
                    var item = await this.itemRepository.GetByIdWithIncludesAsync(itemId);

                    if (item == null)
                    {
                        this.ModelState.AddModelError("ContractItemIsNotExist", $"Товара с идентификатором {itemId} не существует");
                        return this.BadRequest(this.ModelState);
                    }

                    itemsToSet.Add(item);
                }
            }

            trackedEntity.CustomerId = contractDto.CustomerId;
            trackedEntity.DeliveryId = contractDto.DeliveryId;
            trackedEntity.Date = contractDto.Date;

            trackedEntity.Items.Clear();

            foreach (var item in itemsToSet)
            {
                trackedEntity.Items.Add(item);
            }

            await this.contractRepository.UpdateAsync(trackedEntity);

            return this.NoContent();
        }
    }
}
