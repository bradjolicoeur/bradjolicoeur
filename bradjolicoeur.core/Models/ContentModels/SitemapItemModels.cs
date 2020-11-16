using Newtonsoft.Json;
using Squidex.ClientLibrary;
using System;

namespace bradjolicoeur.core.Models.ContentModels
{
    public sealed class SitemapItemData
    {

        [JsonProperty("relativepath")]
        [JsonConverter(typeof(InvariantConverter))]
        public string RelativePath { get; set; }

        [JsonProperty("lastmodified")]
        [JsonConverter(typeof(InvariantConverter))]
        public DateTime LastModified { get; set; }
    }

    public sealed class SitemapItem : Content<SitemapItemData>
    {
    }
}
