using System;
using System.Collections.Generic;

#nullable disable

namespace Natural_Core.Models
{
    public partial class Dsr
    {
        public Dsr()
        {
            Dsrdetails = new HashSet<Dsrdetail>();
        }

        public string Id { get; set; }
        public string Executive { get; set; }
        public string Distributor { get; set; }
        public string Retailor { get; set; }
        public string OrderBy { get; set; }
        public string Area { get; set; }
      
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public decimal TotalAmount { get; set; }
      //  public bool? IsDeleted { get; set; }


        public virtual Distributor DistributorNavigation { get; set; }
        public virtual Executive ExecutiveNavigation { get; set; }
        public virtual Login OrderByNavigation { get; set; }
        public virtual Retailor RetailorNavigation { get; set; }
        public virtual Area AreaNavigation { get; set; }

        public virtual ICollection<Dsrdetail> Dsrdetails { get; set; }
    }
}
