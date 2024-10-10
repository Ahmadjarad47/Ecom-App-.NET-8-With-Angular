using Ecom.Core.Entities;
using Ecom.Core.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.Services
{
    public interface IPaymentService
    {
        Task<CustomerBasket> CreateOrUpdatePaymentAsync(string basketId, int? deliveryId);
        Task<Orders> UpdateOrderSuccess(string PaymentInten);
        Task<Orders> UpdateOrderFaild(string PaymentInten);
    }
}
