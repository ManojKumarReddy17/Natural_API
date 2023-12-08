using System;
using System.Collections.Generic;

#nullable disable

namespace Natural_Core.Models
{
    public partial class UpdateDistributor
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
        public DateTime CreateDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public virtual Area AreaNavigation { get; set; }
        public virtual City CityNavigation { get; set; }
        public virtual State StateNavigation { get; set; }
    }
}
