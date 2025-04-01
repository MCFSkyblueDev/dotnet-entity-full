using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Dtos.Item;
using AutoMapper;
using Core.Entities;
using Core.Dtos.User;

namespace Api.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateItemDto, ItemEntity>();

            CreateMap<ItemEntity, ItemToReturnDto>();

            // CreateMap<RegisterDto, UserEntity>()
            //     .ForMember(dest => dest.Provider, opt => opt.MapFrom(src => "Local"));
        }
    }
}