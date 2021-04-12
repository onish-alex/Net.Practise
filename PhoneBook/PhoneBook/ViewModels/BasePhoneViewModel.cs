namespace PhoneBook.ViewModels
{
    using System;

    public class BasePhoneViewModel
    {
        public Guid Id { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }

        public string StatusName { get; set; }
    }
}
