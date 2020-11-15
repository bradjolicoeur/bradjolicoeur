using bradjolicoeur.core.Models.ContentModels;
using Squidex.ClientLibrary;
using System.Collections.Generic;
using System.Linq;

namespace bradjolicoeur.web.ViewModels
{
    public class ArticleViewModel
    {
        public ContentsResult<BlogArticle, BlogArticleData> ArticleData { get; set; }

        public BlogArticle Article { get => ArticleData?.Items?.FirstOrDefault(); }

        public ContentsResult<BlogArticle, BlogArticleData> Suggestions { get; set; }
    }
}
