using System;
using System.Collections.Generic;

#nullable disable

namespace Natural_Core.Models
{
    public partial class Login
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
