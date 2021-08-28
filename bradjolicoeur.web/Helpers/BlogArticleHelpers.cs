using bradjolicoeur.core.blastcms;
using bradjolicoeur.web.ViewModels;
using System.Linq;

namespace bradjolicoeur.web.Helpers
{
    public static class BlogArticleHelpers
    {
        public static TagsViewModel GetTags(this bradjolicoeur.core.Models.ContentModels.BlogArticleData data)
        {
            return new TagsViewModel(data.BlogTags);
        }

        public static bool HasTags(this bradjolicoeur.core.Models.ContentModels.BlogArticleData data)
        {
            return !(data.BlogTags == null || data.BlogTags.Count() == 0);
        }

        public static TagsViewModel GetTags(this BlogArticle data)
        {
            return new TagsViewModel(data.Tags);
        }

        public static bool HasTags(this BlogArticle data)
        {
            return !(data.Tags == null || data.Tags.Count() == 0);
        }
    }
}
