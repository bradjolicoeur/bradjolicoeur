using bradjolicoeur.core.Models.ContentModels;
using bradjolicoeur.Core.Models.ContentType;
using Squidex.ClientLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bradjolicoeur.web.ViewModels
{
    public class BlogViewModel
    {
        public ContentPage ContentPage { get; set; }

        public ContentsResult<BlogArticle, BlogArticleData> BlogArticles { get; set; }
    }
}
