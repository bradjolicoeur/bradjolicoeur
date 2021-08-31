using System;

namespace bradjolicoeur.core.Models.Web
{
    public class SitemapNode
    {
        public SitemapNode(string url)
        {
            Url = url;
        }
        public SitemapFrequency? Frequency { get; set; }
        public DateTime? LastModified { get; set; }
        public double? Priority { get; set; }
        public string Url { get; set; }
    }

    public enum SitemapFrequency
    {
        Never,
        Yearly,
        Monthly,
        Weekly,
        Daily,
        Hourly,
        Always
    }
}
