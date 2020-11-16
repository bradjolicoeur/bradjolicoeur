using bradjolicoeur.core.Models.ContentModels;
using bradjolicoeur.web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bradjolicoeur.web.Helpers
{
    public static class BlogArticleHelpers
    {
        public static TagsViewModel GetTags(this BlogArticleData data)
        {
            return new TagsViewModel(data.BlogTags);
        }

        public static bool HasTags(this BlogArticleData data)
        {
            return !(data.BlogTags == null || data.BlogTags.Count() == 0);
        }
    }
}
