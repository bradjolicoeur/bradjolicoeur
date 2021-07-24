using System;
using System.Threading.Tasks;
using bradjolicoeur.core.Models.ContentModels;
using LazyCache;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Squidex.ClientLibrary;

namespace bradjolicoeur.web.Pages
{
    public class BlogModel : PageModel
    {
        [FromQuery(Name = "currentpage")]
        public int CurrentPage { get; set; } = 1;

        [FromQuery(Name = "length")]
        public int PageSize { get; set; } = 10;
        public long Count { get; set; }
        public int TotalPages => (int)Math.Ceiling(decimal.Divide(Count, PageSize));
        public bool NextPage => (CurrentPage * PageSize) <= BlogArticles.Total;
        public bool PreviousPage => (CurrentPage > 1);

        private readonly IContentsClient<BlogArticle, BlogArticleData> _blogArticle;
        private readonly IAppCache _appCache;

        public BlogModel( IContentsClient<BlogArticle, BlogArticleData> blogArtcle, IAppCache appCache)
        {
            _blogArticle = blogArtcle;
            _appCache = appCache;
        }

        public ContentsResult<BlogArticle, BlogArticleData> BlogArticles { get; set; }

        public async Task OnGetAsync(string id = null)
        {

            await GetBlogArticles(id);
        }

        private async Task GetBlogArticles(string tag)
        {
            CurrentPage = CurrentPage < 1 ? 1 : CurrentPage;

            var filter = string.IsNullOrEmpty(tag) ? null : $"data/blogtags/iv eq '{tag}'";
            var contentQuery = new ContentQuery
            {
                OrderBy = $"data/publisheddate/iv desc",
                Filter = filter,
                Skip = ((CurrentPage - 1) * PageSize),
                Top = PageSize,
            };

            BlogArticles = await _appCache.GetOrAddAsync(JsonConvert.SerializeObject(contentQuery), () => GetContent(contentQuery));

            Count = BlogArticles.Total;

        }

        private async Task<ContentsResult<BlogArticle, BlogArticleData>> GetContent(ContentQuery contentQuery)
        {
            return await _blogArticle.GetAsync(contentQuery);
        }
    }
}