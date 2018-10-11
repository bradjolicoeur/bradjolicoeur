// This code was generated by a cloud-generators-net tool 
// (see https://github.com/Kentico/cloud-generators-net).
// 
// Changes to this file may cause incorrect behavior and will be lost if the code is regenerated. 
// For further modifications of the class, create a separate file with the partial class.

using System;
using System.Collections.Generic;
using KenticoCloud.Delivery;

namespace bradjolicoeur.Core.Models.ContentType
{
    public partial class EmbedVideo
    {
        public const string Codename = "embed_video";
        public const string HostCodename = "host";
        public const string TitleCodename = "title";
        public const string DescriptionCodename = "description";
        public const string SourceCodename = "source";

        public IEnumerable<MultipleChoiceOption> Host { get; set; }
        public string Title { get; set; }
        public IRichTextContent Description { get; set; }
        public string Source { get; set; }
        public ContentItemSystemAttributes System { get; set; }
    }
}