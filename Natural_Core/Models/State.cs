using System;
using System.Collections.Generic;

#nullable disable

namespace Natural_Core.Models
{
    public partial class State
    {
        public State()
        {
            Cities = new HashSet<City>();
            Distributors = new HashSet<Distributor>();
            Executives = new HashSet<Executive>();
            Retailors = new HashSet<Retailor>();
        }

        public string Id { get; set; }
        public string StateName { get; set; }
        public bool? IsDeleted { get; set; }

        public virtual ICollection<City> Cities { get; set; }
        public virtual ICollection<Distributor> Distributors { get; set; }
        public virtual ICollection<Executive> Executives { get; set; }
        public virtual ICollection<Retailor> Retailors { get; set; }
    }
}
