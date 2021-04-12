using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using App.Core.Entity;
using App.DataAccess.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SimpleApi.DataTransfer.ItemsDto;

namespace SimpleApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private IRepository<Item> itemRepository;
        private IMapper mapper;

        public ItemController(
            IRepository<Item> itemRepository,
            IMapper mapper)
        {
            this.itemRepository = itemRepository;
            this.mapper = mapper;
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(Item), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Get(int id)
        {
            var item = await this.itemRepository.GetByIdWithIncludesAsync(
                id,
                new List<Expression<Func<Item, dynamic>>>()
                {
                    x => x.Contracts,
                },
                false);

            if (item == null)
            {
                this.ModelState.AddModelError("ItemIsNotExist", "Запрашиваемого товара не существует");
                return this.NotFound(this.ModelState["ItemIsNotExist"]);
            }

            foreach (var contract in item.Contracts)
            {
                contract.Items = null;
            }

            return new ObjectResult(item);
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Post(ItemCreateDto itemCreateDto)
        {
            if (this.ModelState.IsValid)
            {
                var itemEntity = this.mapper.Map<Item>(itemCreateDto);
                await this.itemRepository.CreateAsync(itemEntity);
                return this.Created($"/api/Customer/{itemEntity.Id}", null);
            }

            return this.BadRequest(this.ModelState);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Put(int id, ItemDto itemDto)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            if (itemDto.Id != id)
            {
                this.ModelState.AddModelError("ItemWrongId", "Неверно указанный идентификатор");
                return this.BadRequest(this.ModelState);
            }

            var itemEntity = this.mapper.Map<Item>(itemDto);
            var trackedEntity = await this.itemRepository.GetByIdWithIncludesAsync(itemEntity.Id);

            if (trackedEntity == null)
            {
                this.ModelState.AddModelError("ItemIsNotExist", "Запрашиваемого товара не существует");
                return this.BadRequest(this.ModelState);
            }

            trackedEntity.Name = itemEntity.Name;
            trackedEntity.Cost = itemEntity.Cost;
            trackedEntity.Description = itemEntity.Description;
            trackedEntity.Manufacturer = itemEntity.Manufacturer;
            trackedEntity.Nds = itemEntity.Nds;
            trackedEntity.Refrigerate = itemEntity.Refrigerate;

            await this.itemRepository.UpdateAsync(trackedEntity);

            return this.NoContent();
        }
    }
}
