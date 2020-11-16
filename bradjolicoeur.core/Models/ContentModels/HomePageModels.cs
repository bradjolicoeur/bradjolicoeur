using Newtonsoft.Json;
using Squidex.ClientLibrary;
using System.Collections.Generic;

namespace bradjolicoeur.core.Models.ContentModels
{
    public sealed class HomePageData
    {

        [JsonProperty("herotitle")]
        [JsonConverter(typeof(InvariantConverter))]
        public string HeroTitle { get; set; }

        [JsonProperty("herophrase")]
        [JsonConverter(typeof(InvariantConverter))]
        public string HeroPhrase { get; set; }

        [JsonProperty("bio")]
        [JsonConverter(typeof(InvariantConverter))]
        public string Bio { get; set; }

        [JsonProperty("profileimage")]
        [JsonConverter(typeof(InvariantConverter))]
        public IEnumerable<string> ProfileImage { get; set; }
    }

    public sealed class HomePage : Content<HomePageData>
    {
    }
}
