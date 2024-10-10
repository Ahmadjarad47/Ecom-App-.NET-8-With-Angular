using Ecom.Core.Entities;
using Ecom.Core.Entities.Order;
using Ecom.Core.interfaces;
using Ecom.Core.Services;
using Ecom.infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.infrastructure.Repositries.Service
{
    internal class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork work;
        private readonly IConfiguration configuration;
        private readonly AppDbContext _context;
        public PaymentService(IUnitOfWork work, IConfiguration configuration, AppDbContext context)
        {
            this.work = work;
            this.configuration = configuration;
            _context = context;
        }
        public async Task<CustomerBasket> CreateOrUpdatePaymentAsync(string basketId, int? delivertMethodId)
        {
            CustomerBasket basket = await work.CustomerBasket.GetBasketAsync(basketId);
            StripeConfiguration.ApiKey = configuration["StripSetting:secretKey"];
            decimal shippingPrice = 0m;
            if (delivertMethodId.HasValue)
            {
                var delivery = await _context.DeliveryMethods.AsNoTracking()
                    .FirstOrDefaultAsync(m => m.Id == delivertMethodId.Value);
                shippingPrice = delivery.Price;
            }
            foreach (var item in basket.basketItems)
            {
                var product = await work.productRepositry.GetByIdAsync(item.Id);
                item.Price = product.NewPrice;
            }
            PaymentIntentService paymentIntentService = new();
            PaymentIntent _intent;
            if (string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                var option = new PaymentIntentCreateOptions
                {
                    Amount = (long)basket.basketItems.Sum(m => m.Qunatity * (m.Price * 100)) + (long)(shippingPrice * 100),

                    Currency = "USD",
                    PaymentMethodTypes = new List<string> { "card" }
                };
                _intent = await paymentIntentService.CreateAsync(option);
                basket.PaymentIntentId = _intent.Id;
                basket.ClientSecret = _intent.ClientSecret;
            }
            else
            {
                var option = new PaymentIntentUpdateOptions
                {
                    Amount = (long)basket.basketItems.Sum(m => m.Qunatity * (m.Price * 100)) + (long)(shippingPrice * 100),

                };
                await paymentIntentService.UpdateAsync(basket.PaymentIntentId, option);
            }
            await work.CustomerBasket.UpdateBasketAsync(basket);
            return basket;
        }

        public async Task<Orders> UpdateOrderFaild(string PaymentInten)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(m => m.PaymentIntentId == PaymentInten);
            if (order is null)
            {
                return null;
            }
            order.status = Status.PaymentFaild;
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<Orders> UpdateOrderSuccess(string PaymentInten)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(m => m.PaymentIntentId == PaymentInten);
            if (order is null)
            {
                return null;
            }
            order.status = Status.PaymentReceived;
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
            return order;
        }
    }
}
