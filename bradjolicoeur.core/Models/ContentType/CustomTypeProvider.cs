using System;
using KenticoCloud.Delivery;

namespace bradjolicoeur.Core.Models.ContentType
{
    public class CustomTypeProvider : ICodeFirstTypeProvider
    {
        public Type GetType(string contentType)
        {
            switch (contentType)
            {
                case "blog_article":
                    return typeof(BlogArticle);
                case "code_snip":
                    return typeof(CodeSnip);
                case "content_page":
                    return typeof(ContentPage);
                case "embed_video":
                    return typeof(EmbedVideo);
                case "mark_down_content":
                    return typeof(MarkDownContent);
                case "profile":
                    return typeof(Profile);
                case "rich_image":
                    return typeof(RichImage);
                case "socialprofile":
                    return typeof(Socialprofile);
                default:
                    return null;
            }
        }
    }
}