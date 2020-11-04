using AutoMapper;
using Hera.Data.Entities;
using Hera.Services.ViewModels.Authentication;
using Hera.Services.ViewModels.Order;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hera.Services.AutoMapperMapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserEntity, UserViewModel>();
            CreateMap<UserViewModel, UserEntity>();

            CreateMap<OrderEntity, OrderViewModel>();
            CreateMap<OrderViewModel, OrderEntity>();

        }
    }
}
