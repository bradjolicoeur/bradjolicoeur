using bradjolicoeur.core.Models.ContentModels;
using bradjolicoeur.Core.Models.ContentType;
using Squidex.ClientLibrary;

namespace bradjolicoeur.web.ViewModels
{
    public class HomeViewModel
    {
        public ContentPage ContentPage { get; set; }
        public ContentsResult<BlogArticle, BlogArticleData> BlogArticles { get; set; }

    }
}
