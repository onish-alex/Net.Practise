using System;
using System.Collections.Generic;

namespace SimpleApi.DataTransfer.ContractsDto
{
    public class ContractCreateUpdateDto
    {
        public int CustomerId { get; set; }

        public int DeliveryId { get; set; }

        public IEnumerable<int> ItemId { get; set; }

        public DateTime Date { get; set; }
    }
}