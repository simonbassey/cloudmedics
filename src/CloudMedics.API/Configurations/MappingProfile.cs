using System;
using AutoMapper;
using CloudMedics.Domain.ViewModels;
using CloudMedics.Domain.Models;
using CloudMedics.Domain.Enumerations;

namespace CloudMedics.API.Configurations
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<SignUpModel, ApplicationUser>()
                .ForMember(u => u.Created, opt => opt.UseValue(DateTime.Now))
                .ForMember(u => u.CreatedBy, opt => opt.UseValue("SYSTEM"))
                .ForMember(u => u.LastUpdate, opt => opt.UseValue(DateTime.Now))
                .ForMember(u => u.AccountStatus, opt => opt.UseValue(AccountStatus.Disabled))
                .ForMember(u => u.UserName, opt => opt.MapFrom(s => s.Email))
                .ForMember(u => u.AccountType, opt => opt.UseValue(AccountType.Patient));

        }
    }
}
