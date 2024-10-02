using Ecom.Core.Entities.Order;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.infrastructure.Data.Config
{
    public class DeliveryMethodConfiguration : IEntityTypeConfiguration<DeliveryMethod>
    {
        public void Configure(EntityTypeBuilder<DeliveryMethod> builder)
        {
            builder.Property(m => m.Price).HasColumnType("decimal(18,2)");
            builder.HasData(
        new DeliveryMethod { Id = 1, DeliveryTime = "Only a week", Description = "The fast Delivery in the world", Name = "DHL", Price = 15 },
        new DeliveryMethod { Id = 2, DeliveryTime = "Only take two week", Description = "Make your product save", Name = "XXX", Price = 12 }
        );
        }
    }
}
