using System;
using System.Collections.Generic;

#nullable disable

namespace Natural_Core.Models
{
    public partial class Executive
    {
        public Executive()
        {
            DistributorToExecutives = new HashSet<DistributorToExecutive>();
            Dsrs = new HashSet<Dsr>();
            ExecutiveAreas = new HashSet<ExecutiveArea>();
            ExecutiveGps = new HashSet<ExecutiveGp>();
            NotificationExecutives = new HashSet<NotificationExecutive>();
        }

        public string Id { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Image { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public bool? IsDeleted { get; set; }

        public virtual City CityNavigation { get; set; }
        public virtual State StateNavigation { get; set; }
        public virtual ICollection<DistributorToExecutive> DistributorToExecutives { get; set; }
        public virtual ICollection<Dsr> Dsrs { get; set; }
        public virtual ICollection<ExecutiveArea> ExecutiveAreas { get; set; }
        public virtual ICollection<ExecutiveGp> ExecutiveGps { get; set; }
        public virtual ICollection<NotificationExecutive> NotificationExecutives { get; set; }
    }
}
