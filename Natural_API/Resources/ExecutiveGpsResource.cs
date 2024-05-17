using Natural_Core.Models;

namespace Natural_API.Resources
{
    public class ExecutiveGpsResource
    {
        public int Id { get; set; }
        public string ExecutiveId { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }

       // public virtual Executive Executive { get; set; }
    }
}
