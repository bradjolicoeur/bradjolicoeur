using Newtonsoft.Json;
using Squidex.ClientLibrary;

namespace bradjolicoeur.core.Models.ContentModels
{
    public sealed class ResumeData
    {

        [JsonProperty("body")]
        [JsonConverter(typeof(InvariantConverter))]
        public string Body { get; set; }
    }

    public sealed class Resume : Content<ResumeData>
    {
    }
}
