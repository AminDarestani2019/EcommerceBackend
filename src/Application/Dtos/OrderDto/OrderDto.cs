using Application.Common.Mapping;
using Application.Dtos.Common;
using AutoMapper;
using Domain.Entities.Order;
using Domain.Enums;

namespace Application.Dtos.OrderDto
{
    public class OrderDto:BaseAuditableEntityDtoResponse,IMapFrom<Order>
    {
        public string BuyerPhoneNumber { get; set; }
        public decimal SubTotal { get; set; }
        public string TrackingCode { get; set; }
        public bool IsFinally { get; set; }
        public decimal Total { get; set; }
        //portal
        public PortalDto Portal { get; set; }
        public PortalType PortalType { get; set; } 
        public string Authority { get; set; }
        public string Link { get; set; }//gateway link
        public int Status { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Order, OrderDto>()
                .ForMember(x=>x.Status,c=>c.MapFrom(v=>(int)v.OrderSTatus))
                .ForMember(x => x.Total, c => c.MapFrom(v => v.GetOriginalTotal()));
        }
        public ShipToAddress ShipToAddress { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }
    }
}
