using System;
using System.Collections.Generic;
using System.Text;

namespace Natural_Core.Models
{
    public class GetRetailor
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string Address { get; set; }
        public string Area { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PresignedUrl { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }
}
