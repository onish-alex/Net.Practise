using App.Core.Entity;
using AutoMapper;
using SimpleApi.DataTransfer.ContractsDto;
using SimpleApi.DataTransfer.CustomersDto;
using SimpleApi.DataTransfer.DeliveriesDto;
using SimpleApi.DataTransfer.ItemsDto;

namespace SimpleApi.MappingProfiles
{
    public class MainProfile : Profile
    {
        public MainProfile()
        {
            // Customer mapping
            this.CreateMap<CustomerCreateDto, Customer>()
                .Include<CustomerDto, Customer>();

            this.CreateMap<CustomerDto, Customer>().ReverseMap();

            // Item mapping
            this.CreateMap<ItemCreateDto, Item>()
                .Include<ItemDto, Item>();

            this.CreateMap<Item, ItemDto>().ReverseMap();

            // Delivery mapping
            this.CreateMap<DeliveryCreateDto, Delivery>()
                .Include<DeliveryDto, Delivery>();

            this.CreateMap<DeliveryDto, Delivery>().ReverseMap();

            // Contract mapping
            this.CreateMap<ContractCreateUpdateDto, Contract>();
        }
    }
}
