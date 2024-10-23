using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce
{
    public enum Role
    {
        ADMIN,
        CUSTOMER
    }

    public enum OrderStatus
    {
        Pending,
        Processed,
        Shipped,
        Delivered,
        Cancelled
    }

    public struct Address
    {
        public string Street { get; set; }
        public string City { get; set; }
        public long? ZipCode { get; set; }

        public Address(string street, string city, long zipCode)
        {
            Street = street;
            City = city;
            ZipCode = zipCode;
        }

        public override string ToString()
        {
            return $"{Street}, {City}, {ZipCode}";
        }
    }
}
