using Natural_Core.Models;

namespace Natural_API.Resources
{
    public class CityUpdateResource
    {
        public string CityName { get; set; }
        public virtual State State { get; set; }
    }
}
