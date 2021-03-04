namespace PhoneBook.ViewModels
{
    using System;

    public class PhoneViewModel
    {
        public Guid Id { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime LastUpdateDate { get; set; }

        public string Status { get; set; }

        public Guid CreatorId { get; set; }
    }
}
