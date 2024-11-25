using AutoMapper;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.ServiceModels;
using Microsoft.Extensions.DependencyInjection;
using ASI.Basecode.WebApp.Models;

namespace ASI.Basecode.WebApp
{
    // AutoMapper configuration
    internal partial class StartupConfigurer
    {
        /// <summary>
        /// Configures AutoMapper and adds it to the service container.
        /// </summary>
        private void ConfigureAutoMapper()
        {
            var mapperConfiguration = new MapperConfiguration(config =>
            {
                config.AddProfile(new AutoMapperProfileConfiguration());
            });

            _services.AddSingleton<IMapper>(sp => mapperConfiguration.CreateMapper());
        }

        /// <summary>
        /// AutoMapper profile configuration for mapping entities and view models.
        /// </summary>
        private class AutoMapperProfileConfiguration : Profile
        {
            public AutoMapperProfileConfiguration()
            {
                // Mapping for LoginViewModel to MUser
                CreateMap<Services.ServiceModels.LoginViewModel, MUser>()
                    .ForMember(dest => dest.UserCode, opt => opt.MapFrom(src => src.UserId))
                    .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
                    .ReverseMap();

                // Mapping for UserViewModel to MUser
                CreateMap<UserViewModel, MUser>()
                    .ForMember(dest => dest.UserCode, opt => opt.MapFrom(src => src.UserCode))
                    .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                    .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                    .ForMember(dest => dest.Mail, opt => opt.MapFrom(src => src.Email))
                    .ForMember(dest => dest.UserRole, opt => opt.MapFrom(src => src.Role))
                    .ReverseMap();

                // Mapping for Room to RoomViewModel
                CreateMap<Room, RoomViewModel>()
                    .ForMember(dest => dest.RoomId, opt => opt.MapFrom(src => src.Id)) // Mapping Id to RoomId
                    .ReverseMap();

                // Mapping for Booking to BookingViewModel
                CreateMap<Booking, BookingViewModel>()
                    .ForMember(dest => dest.BookingId, opt => opt.MapFrom(src => src.Id)) // Mapping Id to BookingId
                    .ReverseMap();
            }
        }
    }
}
