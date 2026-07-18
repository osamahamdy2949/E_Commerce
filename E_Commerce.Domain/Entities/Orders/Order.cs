using E_Commerce.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Entities.Orders
{
    public class Order : BaseEntity<Guid>
    {
        private Order()
        {
            
        }
        public Order(string bayerEmail, OrderAddress shippingAddress, 
            ICollection<OrderItem> items, DeliveryMethod deliveryMethod, decimal subTotal)
        {
            BayerEmail = bayerEmail;
            ShippingAddress = shippingAddress;
            Items = items;
            DeliveryMethod = deliveryMethod;
            SubTotal = subTotal;
        }

        public string BayerEmail { get; set; } = default!;
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.UtcNow;
        public OrderAddress ShippingAddress { get; set; } = default!;

        public ICollection<OrderItem> Items { get; set; } = [];
        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        public DeliveryMethod DeliveryMethod { get; set; } = default!;
        public int DeliveryMethodId { get; set; }

        public decimal SubTotal { get; set; }

        public decimal GetTotal() => SubTotal + (DeliveryMethod?.Cost ?? 0);
    }
}
