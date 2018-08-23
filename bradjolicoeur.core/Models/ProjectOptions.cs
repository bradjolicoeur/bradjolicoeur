using KenticoCloud.Delivery;

namespace bradjolicoeur.core.Models
{
    public class ProjectOptions
    {
        public DeliveryOptions DeliveryOptions { get; set; }
        public int CacheTimeoutSeconds { get; set; }
    }
}
