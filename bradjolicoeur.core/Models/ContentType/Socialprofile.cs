﻿// This code was generated by a cloud-generators-net tool 
// (see https://github.com/Kentico/cloud-generators-net).
// 
// Changes to this file may cause incorrect behavior and will be lost if the code is regenerated. 
// For further modifications of the class, create a separate file with the partial class.

using System;
using System.Collections.Generic;
using KenticoCloud.Delivery;

namespace bradjolicoeur.Core.Models.ContentType
{
    public partial class Socialprofile
    {
        public const string Codename = "socialprofile";
        public const string UrlCodename = "url";
        public const string TypeCodename = "type";

        public string Url { get; set; }
        public IEnumerable<MultipleChoiceOption> Type { get; set; }
        public ContentItemSystemAttributes System { get; set; }
    }
}