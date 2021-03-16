namespace SimpleApi.DataTransfer.ItemsDto
{
    public class ItemCreateDto
    {
        public string Name { get; set; }

        public decimal Cost { get; set; }

        public string Nds { get; set; }

        public string Description { get; set; }

        public string Manufacturer { get; set; }

        public bool? Refrigerate { get; set; }
    }
}