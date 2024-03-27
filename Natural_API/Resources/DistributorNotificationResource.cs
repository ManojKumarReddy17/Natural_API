using System;
using Natural_Core.Models;

#nullable disable

namespace Natural_API.Resources
{
	public class DistributorNotificationResource
	{
       
            public int Id { get; set; }
            public string Distributor { get; set; }
            public string Subject { get; set; }
            public string Body { get; set; }
            //public DateTime Date { get; set; }
            public DateTime CreatedDate { get; set; }
            public DateTime ModifiedDate { get; set; }



    }
}

