namespace PhoneBook.Models
{
    using System;
    using System.Collections.Generic;

    public class User
    {
        public Guid Id { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public virtual ICollection<BookEntry> Entries { get; set; }
    }
}
