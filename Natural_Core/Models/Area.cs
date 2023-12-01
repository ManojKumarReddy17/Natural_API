using System;
using System.Collections.Generic;

#nullable disable

namespace Natural_Core.Models
{
    public partial class Area
    {
        public Area()
        {
            Distributors = new HashSet<Distributor>();
            Executives = new HashSet<Executive>();
            Retailors = new HashSet<Retailor>();
        }

        public string Id { get; set; }
        public string AreaName { get; set; }
        public string CityId { get; set; }

        public virtual City City { get; set; }
        public virtual ICollection<Distributor> Distributors { get; set; }
        public virtual ICollection<Executive> Executives { get; set; }
        public virtual ICollection<Retailor> Retailors { get; set; }
    }
}
