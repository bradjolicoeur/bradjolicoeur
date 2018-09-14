using bradjolicoeur.Core.Models.ContentType;
using System.Collections.Generic;

namespace bradjolicoeur.web.ViewModels
{
    public class ArticleViewModel
    {
        public BlogArticle Article { get; set; }

        public IEnumerable<BlogArticle> Suggestions { get; set; }
    }
}
