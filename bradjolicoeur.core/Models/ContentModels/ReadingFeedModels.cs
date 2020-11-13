using Newtonsoft.Json;
using Squidex.ClientLibrary;
using System;
using System.Collections.Generic;
using System.Text;

namespace bradjolicoeur.core.Models.ContentModels
{
    public sealed class ReadingFeedData
    {

        [JsonConverter(typeof(InvariantConverter))]
        public string Slug { get; set; }

        [JsonConverter(typeof(InvariantConverter))]
        public string Title { get; set; }

        [JsonProperty("articleurl")]
        [JsonConverter(typeof(InvariantConverter))]
        public string ArticleUrl { get; set; }

        [JsonProperty("imageurl")]
        [JsonConverter(typeof(InvariantConverter))]
        public string ImageUrl { get; set; }

        [JsonConverter(typeof(InvariantConverter))]
        public string Author { get; set; }

        [JsonConverter(typeof(InvariantConverter))]
        public string Description { get; set; }

        [JsonConverter(typeof(InvariantConverter))]
        public string Notes { get; set; }

        [JsonProperty("sitename")]
        [JsonConverter(typeof(InvariantConverter))]
        public string SiteName { get; set; }

        [JsonProperty("dateposted")]
        [JsonConverter(typeof(InvariantConverter))]
        public DateTime DatePosted { get; set; }
    }

    public sealed class ReadingFeed : Content<ReadingFeedData>
    {
    }
}
