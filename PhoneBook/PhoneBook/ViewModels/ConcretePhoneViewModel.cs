namespace PhoneBook.ViewModels
{
    using System;

    public class ConcretePhoneViewModel : BasePhoneViewModel
    {
        public bool IsCreator { get; set; }

        public string Address { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime LastUpdateDate { get; set; }
    }
}
