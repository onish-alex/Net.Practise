using SimpleApi.Enumeration;

namespace SimpleApi.DataTransfer.DeliveriesDto
{
    public class DeliveryCreateDto
    {
        public string Address { get; set; }

        public DeliveryType TypeDelivery { get; set; }
    }
}