using System;
using System.Collections.Generic;
using System.Text;

namespace Natural_Core.Models
{
    public class AngularDistributor
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string Address { get; set; }




        public int Statuscode { get; set; }
        public string Message { get; set; }

        // public virtual ICollection<DistributorToExecutive> DistributorToExecutives { get; set; }
        //public virtual DistributorToExecutive DistributorToExecutives { get; set; }




        public string Executives { get; set; }


        public string ExeId { get; set; }
    }
}
