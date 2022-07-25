using System;
using System.Collections.Generic;
using ServiceStack.DataAnnotations;
using System.Text;

namespace SMSService.Core.Models
{
    internal class Country
    {
        [AutoId]
        public Guid Id { get; set; }
        public string Name { get; set; }
        [Unique]
        public string MobileCountryCode { get; set; }
        [Unique]
        public string CountryCode { get; set; }
        public decimal PricePerSMS { get; set; }
    }
}
