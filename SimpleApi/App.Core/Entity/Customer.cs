namespace App.Core.Entity
{
    public class Customer : BaseEntity
    {
        public string FullName { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }
    }
}
