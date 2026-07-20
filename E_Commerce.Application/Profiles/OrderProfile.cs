using AutoMapper;
using E_Commerce.Application.DTOs.Identity;
using E_Commerce.Application.DTOs.Orders;
using E_Commerce.Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Profiles
{
    internal class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<AddressDto, OrderAddress>().ReverseMap();
            
            CreateMap<Order, OrderToReturnDto>()
                .ForMember(o => o.DeliveryMethod, x => x.MapFrom(o => o.DeliveryMethod.ShortName))
                .ForMember(o => o.DeliveryMethodCost, x => x.MapFrom(o => o.DeliveryMethod.Cost));

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(o => o.ProductId, x => x.MapFrom(o => o.Product.ProductId))
                .ForMember(o => o.ProductName, x => x.MapFrom(o => o.Product.ProductName))
                .ForMember(o => o.PictureUrl, x => x.MapFrom<OrderItemPictureUrlResolver>());
        }
    }
}
