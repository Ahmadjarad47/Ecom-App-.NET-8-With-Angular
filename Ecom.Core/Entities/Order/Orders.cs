using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.Entities.Order
{
    public class Orders : BaseEntity<int>
    {
        public Orders() { }
        public Orders(string buyerEmail, decimal subTotal, ShippingAddress shippingAddress, DeliveryMethod deliveryMethod, IReadOnlyList<OrderItem> orderItems, string PaymentIntentId)
        {
            BuyerEmail = buyerEmail;
            SubTotal = subTotal;
            this.shippingAddress = shippingAddress;
            this.deliveryMethod = deliveryMethod;
            this.orderItems = orderItems;
            this.PaymentIntentId = PaymentIntentId;
        }

        public string BuyerEmail { get; set; }
        public decimal SubTotal { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public ShippingAddress shippingAddress { get; set; }
        public string PaymentIntentId { get; set; }
        public IReadOnlyList<OrderItem> orderItems { get; set; }
        public DeliveryMethod deliveryMethod { get; set; }


        public Status status { get; set; } = Status.Pending;

        public decimal GetTotal()
        {
            return SubTotal + deliveryMethod.Price;
        }

    }
}
