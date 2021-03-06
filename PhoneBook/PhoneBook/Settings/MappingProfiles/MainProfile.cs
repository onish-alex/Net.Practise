namespace PhoneBook.Settings.MappingProfiles
{
    using AutoMapper;
    using PhoneBook.Models;
    using PhoneBook.ViewModels;

    public class MainProfile : Profile
    {
        public MainProfile()
        {
            this.CreateMap<BookEntry, BasePhoneViewModel>()
                .ForMember(dest => dest.StatusName, opt => opt.MapFrom(x => x.Status.Name));

            this.CreateMap<BookEntry, ConcretePhoneViewModel>()
                .IncludeBase<BookEntry, BasePhoneViewModel>();

            this.CreateMap<BookEntry, EditPhoneViewModel>()
                .IncludeBase<BookEntry, BasePhoneViewModel>();

            this.CreateMap<CreatePhoneViewModel, BookEntry>();
        }
    }
}
