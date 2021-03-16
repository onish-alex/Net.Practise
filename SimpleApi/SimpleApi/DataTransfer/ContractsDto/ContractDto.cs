using System.Collections.Generic;
using SimpleApi.DataTransfer.CustomersDto;
using SimpleApi.DataTransfer.DeliveriesDto;
using SimpleApi.DataTransfer.ItemsDto;

namespace SimpleApi.DataTransfer.ContractsDto
{
    public class ContractDto
    {
        public int Id { get; set; }

        public IEnumerable<ItemDto> ItemDto { get; set; }

        public DeliveryDto DeliveryDto { get; set; }

        public CustomerDto CustomerDto { get; set; }
    }
}