﻿#nullable disable

namespace Natural_API.Resources
{
    public class ExecutiveGetResource
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string Address { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public List<string> Area { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
       // public string PresignedUrl { get; set; }
        public object Image { get; internal set; }
    }
}
