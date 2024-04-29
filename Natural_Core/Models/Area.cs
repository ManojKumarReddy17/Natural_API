using System;
using System.Collections.Generic;

#nullable disable

namespace Natural_Core.Models
{
    public partial class Area
    {
        public Area()
        {
            DistributorbyAreas = new HashSet<DistributorbyArea>();
            Distributors = new HashSet<Distributor>();
            ExecutiveAreas = new HashSet<ExecutiveArea>();
            Executives = new HashSet<Executive>();
            Retailors = new HashSet<Retailor>();
        }

        public string Id { get; set; }
        public string AreaName { get; set; }
        public string CityId { get; set; }
        public bool? IsDeleted { get; set; }

        public virtual City City { get; set; }
        public virtual ICollection<DistributorbyArea> DistributorbyAreas { get; set; }
        public virtual ICollection<Distributor> Distributors { get; set; }
        public virtual ICollection<Executive> Executives { get; set; }
        public virtual ICollection<Retailor> Retailors { get; set; }
        public virtual ICollection<ExecutiveArea> ExecutiveAreas { get; set; }
    }
}
