﻿using System;
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
            Executives = new HashSet<ExecutiveGetResourcecs>();
            Retailors = new HashSet<Retailor>();
            ExecutiveAreas = new HashSet<ExecutiveArea>();
        }

        public string Id { get; set; }
        public string AreaName { get; set; }
        public string CityId { get; set; }
        public bool? IsDeleted { get; set; }

        public virtual City City { get; set; }
        public virtual ICollection<DistributorbyArea> DistributorbyAreas { get; set; }
        public virtual ICollection<Distributor> Distributors { get; set; }
        public virtual ICollection<ExecutiveGetResourcecs> Executives { get; set; }
        public virtual ICollection<Retailor> Retailors { get; set; }
        public virtual ICollection<ExecutiveArea> ExecutiveAreas { get; set; }
    }
}
