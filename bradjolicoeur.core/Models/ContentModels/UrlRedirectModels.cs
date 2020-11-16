using Newtonsoft.Json;
using Squidex.ClientLibrary;
using System;
using System.Collections.Generic;
using System.Text;

namespace bradjolicoeur.core.Models.ContentModels
{
    public sealed class UrlRedirectData
    {

        [JsonProperty("redirectfrom")]
        [JsonConverter(typeof(InvariantConverter))]
        public string RedirectFrom { get; set; }

        [JsonProperty("redirectto")]
        [JsonConverter(typeof(InvariantConverter))]
        public string RedirectTo { get; set; }
    }

    public sealed class UrlRedirect : Content<UrlRedirectData>
    {
    }
}
