namespace Dapper.Practice.UserAnonimizer
{
    using System;

    public class MailContent
    {
        public string To { get; set; }

        public string From { get; set; }

        public string Subject { get; set; }

        public string Letter { get; set; }

        public DateTime Date { get; set; }
    }
}
