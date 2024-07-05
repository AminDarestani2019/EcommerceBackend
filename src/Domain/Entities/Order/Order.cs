using Domain.Entities.Base;
using Domain.Entities.Identity;
using Domain.Enums;

namespace Domain.Entities.Order
{
    public class Order:BaseAuditableEntity
    {
        public string BuyerPhoneNumber {  get; set; }
        public decimal SubTotal { get; set; }
        //order status
        public OrderStatus OrderSTatus { get; set; } = OrderStatus.Pending;
        public string TrackingCode { get; set; }
        //portal
        public Portal Portal { get; set; }
        public PortalType PortalType { get; set; } = PortalType.ZarrinPal;
        public bool IsFinally { get; set; } = false;
        public string Authority { get; set; }
        public decimal GetOriginalTotal()
        { 
            return SubTotal+DeliveryMethod.Price;
        }
        public List<OrderItem> OrderItems { get; set; } = new();
        public ShipToAddress ShipToAddress { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }
        public User User { get; set; }
    }
}
