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
    public partial class ContentPage
    {
        public const string Codename = "content_page";
        public const string TitleCodename = "title";
        public const string RouteCodename = "route";
        public const string BodyCodename = "body";

        public string Title { get; set; }
        public string Route { get; set; }
        public IRichTextContent Body { get; set; }
        public ContentItemSystemAttributes System { get; set; }
    }
}