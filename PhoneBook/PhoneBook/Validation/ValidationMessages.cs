namespace PhoneBook.Validation
{
    using PhoneBook.Settings;

    public static class ValidationMessages
    {
        public static string UserRegisterNull = "Заполните все поля для регистрации!";
        public static string UserLoginNull = "Необходимо указать логин";
        public static string UserPasswordNull = "Необходимо указать пароль";
        public static string UserConfirmPasswordNotMatch = "Пароли должны совпадать!";
        public static string AddressNull = "Адрес не может быть пустым";

        public static string UserLoginLength => string.Format(
            "Логин должен быть длиной от {0} до {1} символов!",
            DataSettings.UserLoginMinLength,
            DataSettings.UserLoginMaxLength);

        public static string UserPasswordLength => string.Format(
            "Пароль должен быть длиной от {0} до {1} символов!",
            DataSettings.UserPasswordMinLength,
            DataSettings.UserPasswordMaxLength);

        public static string PhoneNumberFormat => string.Format(
            "Номер телефона должен быть записан в виде {0}",
            DataSettings.PhoneNumberFormat);

        public static string AddressLength => string.Format(
            "Длина адреса не должна превышать {0} символов",
            DataSettings.AddressMaxLength);
    }
}
