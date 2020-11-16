using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bradjolicoeur.core.Models.ContentModels;
using bradjolicoeur.Core.Models.ContentType;
using bradjolicoeur.web.ViewModels;
using KenticoCloud.Delivery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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

        public BlogModel( IContentsClient<BlogArticle, BlogArticleData> blogArtcle)
        {
            _blogArticle = blogArtcle;
        }

        public ContentsResult<BlogArticle, BlogArticleData> BlogArticles { get; set; }

        public async Task OnGetAsync(string id = null)
        {

            await GetBlogArticles(id);
        }

        private async Task GetBlogArticles(string tag)
        {
            var filter = string.IsNullOrEmpty(tag) ? null : $"data/blogtags/iv eq '{tag}'";

            BlogArticles = await _blogArticle.GetAsync(new ContentQuery
            {
                OrderBy = $"data/publisheddate/iv desc",
                Filter = filter,
                Skip = ((CurrentPage - 1) * PageSize),
                Top = PageSize,
            });

            Count = BlogArticles.Total;

        }
    }
}