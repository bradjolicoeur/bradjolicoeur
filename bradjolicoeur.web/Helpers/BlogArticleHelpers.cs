using bradjolicoeur.core.blastcms;
using bradjolicoeur.web.ViewModels;
using System.Linq;

namespace bradjolicoeur.web.Helpers
{
    public static class BlogArticleHelpers
    {
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
