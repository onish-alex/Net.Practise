namespace PhoneBook.ViewModels
{
    using System;
    using System.Collections.Generic;

    public class EditPhoneViewModel : CreatePhoneViewModel
    {
        public Guid CreatorId { get; set; }
    }
}
