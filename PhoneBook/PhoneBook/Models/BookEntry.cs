namespace PhoneBook.Models
{
    using System;

    public class BookEntry
    {
        public Guid Id { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime LastUpdateDate { get; set; }

        public Guid StatusId { get; set; }

        public BookEntryStatus Status { get; set; }

        public Guid CreatorId { get; set; }

        public virtual User Creator { get; set; }
    }
}
