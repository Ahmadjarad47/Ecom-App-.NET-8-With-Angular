using AutoMapper;
using Ecom.Core.DTO;
using Ecom.Core.Entities.Order;

namespace Ecom.API.Mapping
{
    public class OrderMapping : Profile
    {
        public OrderMapping()
        {
            CreateMap<ShippingAddress, ShipAddressDTO>().ReverseMap();
        }
    }
}
