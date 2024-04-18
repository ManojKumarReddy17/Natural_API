using System;

#nullable disable
namespace Natural_API.Resources
{
	public class NotificationResource
	{
        public string Id { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        public List<NotificationDistributorResource> distributorlist { get; set; }
        public List<NotificationExecutiveResource> executiveList { get; set; }

    }
}

