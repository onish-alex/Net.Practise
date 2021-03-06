namespace PhoneBook.ViewModels
{
    using System.Collections.Generic;

    public class CreatePhoneViewModel : BasePhoneViewModel
    {
        public IEnumerable<string> StatusNames { get; set; }
    }
}
