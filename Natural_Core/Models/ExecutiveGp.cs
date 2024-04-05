using System;
using System.Collections.Generic;



namespace Natural_Core.Models
{
    public partial class ExecutiveGp
    {
        public int Id { get; set; }
        public string ExecutiveId { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }

        public virtual Executive Executive { get; set; }
    }
}
