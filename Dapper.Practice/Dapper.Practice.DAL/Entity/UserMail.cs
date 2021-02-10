namespace Dapper.Practice.DAL.Entity
{
    using System.ComponentModel.DataAnnotations.Schema;

    public class UserMail
    {
        [Column("User_Id")]
        public int UserId { get; set; }

        [Column("Mail_Id")]
        public int MailId { get; set; }
    }
}
