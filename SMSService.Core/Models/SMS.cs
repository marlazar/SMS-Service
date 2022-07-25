using System;
using System.Collections.Generic;
using ServiceStack.DataAnnotations;
using System.Text;

namespace SMSService.Core.Models
{
    public class SMS
    {
        [AutoId]
        public Guid Id { get; set; }
        [Required]
        public string Sender { get; set; }
        [Required]
        public string Receiver { get; set; }
        [StringLength(StringLengthAttribute.MaxText)]
        public string Text { get; set; }
        public DateTime SendDate { get; set; }
        public string MobileCountryCode { get; set; }
    }
}
