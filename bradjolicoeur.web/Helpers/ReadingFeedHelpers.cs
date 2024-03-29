﻿using bradjolicoeur.core.blastcms;
using System;
using System.Text;
using System.Web;

namespace bradjolicoeur.web.Helpers
{
    public static class ReadingFeedHelpers
    {
        public static string AuthorAndSiteName(this FeedArticle data)
        {
            var sb = new StringBuilder();

            sb.Append(data.Author);

            if (!string.IsNullOrEmpty(data.Author) && !string.IsNullOrEmpty(data.SiteName))
                sb.Append(" - ");

            sb.Append(data.SiteName?.Replace("www.", "", StringComparison.OrdinalIgnoreCase));

            return sb.ToString();
        }

        public static bool IsYouTube(this FeedArticle data)
        {
            return data.ArticleUrl.Contains("youtu", StringComparison.OrdinalIgnoreCase);
        }

        public static string YouTubeEmbeddedUrl(this FeedArticle data)
        {
            string param1 = "";
            if(data.ArticleUrl.Contains("https://youtu.be/",StringComparison.OrdinalIgnoreCase))
            {
                param1 = data.ArticleUrl.Replace("https://youtu.be/","",StringComparison.OrdinalIgnoreCase);
            } else
            {
                Uri myUri = new Uri(data.ArticleUrl);
                param1 = HttpUtility.ParseQueryString(myUri.Query).Get("v");
            }
            

            return $"https://www.youtube.com/embed/" + param1;
        }

        public static string ButtonLabel(this FeedArticle data)
        {
            return data.IsYouTube() ? "View" : "Read More";
        }
    }
}
