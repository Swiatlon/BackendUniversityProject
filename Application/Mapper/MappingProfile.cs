using Application.Dtos;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Account, AccountFullDto>().ReverseMap();
            CreateMap<Employee, EmployeeFullDto>().ReverseMap();
            CreateMap<Address, AddressFullDto>().ReverseMap();

            CreateMap<Account, AccountOnlyDto>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
                .ForMember(dest => dest.DeactivationDate, opt => opt.MapFrom(src => src.DeactivationDate))
                .ReverseMap();

            CreateMap<Employee, EmployeeOnlyDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.Surname))
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate))
                .ForMember(dest => dest.Pesel, opt => opt.MapFrom(src => src.Pesel))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender))
                .ReverseMap();

            CreateMap<Address, AddressOnlyDto>()
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City))
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Street))
                .ForMember(dest => dest.HouseNumber, opt => opt.MapFrom(src => src.HouseNumber))
                .ForMember(dest => dest.ApartmentNumber, opt => opt.MapFrom(src => src.ApartmentNumber))
                .ForMember(dest => dest.PostalCode, opt => opt.MapFrom(src => src.PostalCode))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country))
                .ReverseMap();
        }
    }
}
