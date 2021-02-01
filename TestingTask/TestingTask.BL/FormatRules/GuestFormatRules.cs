namespace TestingTask.BL.FormatRules
{
    public static class GuestFormatRules
    {
        public const int FirstNameMinLength = 4;
        public const int FirstNameMaxLength = 20;

        public const int LastNameMinLength = 4;
        public const int LastNameMaxLength = 20;

        public const int ChildMinAge = 0;
        public const int ChildMaxAge = 5;

        public const int AdultMinAge= 6;
        public const int AdultMaxAge = 120;

        public const string AllowableSymbols = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
    }
}
