namespace PhoneBook.Settings
{
    using PhoneBook.Models;
    using PhoneBook.ViewModels;

    public static class MapperConfiguration
    {
        public static AutoMapper.MapperConfiguration Configuration { get; } = new AutoMapper.MapperConfiguration(options =>
        {
            options.CreateMap<BookEntry, PhoneViewModel>().ReverseMap();
        });
    }
}
