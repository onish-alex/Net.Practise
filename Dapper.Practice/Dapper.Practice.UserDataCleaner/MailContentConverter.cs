namespace Dapper.Practice.UserAnonimizer
{
    using Newtonsoft.Json;

    public static class MailContentConverter
    {
        public static MailContent FromJSON(string mailContentJson)
        {
            return JsonConvert.DeserializeObject<MailContent>(mailContentJson);
        }

        public static string ToJSON(MailContent mailContent)
        {
            return JsonConvert.SerializeObject(mailContent);
        }
    }
}
