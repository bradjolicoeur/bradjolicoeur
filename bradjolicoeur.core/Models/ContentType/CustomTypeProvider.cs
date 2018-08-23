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
                case "mark_down_content":
                    return typeof(MarkDownContent);
                case "content_page":
                    return typeof(ContentPage);
                default:
                    return null;
            }
        }
    }
}