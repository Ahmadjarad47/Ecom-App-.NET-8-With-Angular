using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.DTO
{
    public record OrderDTO
    {
        public int deliveryMethodId { get; set; }

        public string basketId { get; set; }
        public ShipAddressDTO shipAddress { get; set; }
    }
    public record ShipAddressDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string Street { get; set; }
        public string State { get; set; }
    }
}
