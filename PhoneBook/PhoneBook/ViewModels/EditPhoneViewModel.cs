namespace PhoneBook.ViewModels
{
    using System;

    public class EditPhoneViewModel : CreatePhoneViewModel
    {
        public Guid CreatorId { get; set; }
    }
}
