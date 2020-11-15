using Newtonsoft.Json;
using Squidex.ClientLibrary;
using System;
using System.Collections.Generic;

namespace bradjolicoeur.core.Models.ContentModels
{
    public sealed class BlogArticleData
    {

        [JsonConverter(typeof(InvariantConverter))]
        public string Title { get; set; }

        [JsonConverter(typeof(InvariantConverter))]
        public string Author { get; set; }

        [JsonProperty("publisheddate")]
        [JsonConverter(typeof(InvariantConverter))]
        public DateTime PublishedDate { get; set; }

        [JsonProperty("image")]
        [JsonConverter(typeof(InvariantConverter))]
        public string[] ImageUrl { get; set; }

        [JsonProperty("blogtags")]
        [JsonConverter(typeof(InvariantConverter))]
        public IEnumerable<string> BlogTags { get; set; }

        [JsonConverter(typeof(InvariantConverter))]
        public string Description { get; set; }

        [JsonConverter(typeof(InvariantConverter))]
        public string Body { get; set; }

        [JsonConverter(typeof(InvariantConverter))]
        public string Slug { get; set; }

        [JsonIgnore]
        public string FriendlyUrl { get => Slug; }

    }

    public sealed class BlogArticle : Content<BlogArticleData>
    {
    }
}
