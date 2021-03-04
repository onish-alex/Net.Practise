namespace PhoneBook.ViewModels
{
    using System.Collections.Generic;

    public class PhoneListViewModel
    {
        public IList<PhoneViewModel> Phones { get; set; }

        public int Page { get; set; }

        public int MinPage { get; set; }

        public int MaxPage { get; set; }
    }
}
