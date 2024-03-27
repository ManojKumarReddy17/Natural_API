using System;
using System.Collections.Generic;

#nullable disable

namespace Natural_Core.Models
{
    public partial class Login
    {
        public Login()
        {
            Dsrs = new HashSet<Dsr>();
        }

        public string Id { get; set; }
        public string UserName { get; set; }
        public byte[] Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool? IsDeleted { get; set; }

        public virtual ICollection<Dsr> Dsrs { get; set; }
    }
}
