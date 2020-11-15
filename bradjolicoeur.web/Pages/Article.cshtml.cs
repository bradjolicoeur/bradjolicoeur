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
    public class ArticleModel : PageModel
    {
        private readonly IContentsClient<BlogArticle, BlogArticleData> _blogArticle;

        public ISquidexClientManager _squidex { get; }

        public ArticleModel(IContentsClient<BlogArticle, BlogArticleData> blogArtcle, ISquidexClientManager squidex)
        {
            _blogArticle = blogArtcle;
            _squidex = squidex;
        }

        public ContentsResult<BlogArticle, BlogArticleData> ArticleData { get; set; }

        public BlogArticle Article { get => ArticleData?.Items?.FirstOrDefault(); }

        public string ImageUrl { get => _squidex.GenerateImageUrl(Article.Data.ImageUrl.FirstOrDefault()); }

        public ContentsResult<BlogArticle, BlogArticleData> Suggestions { get; set; }

        public async Task<IActionResult> OnGet(string slug)
        {
            if (slug == null)
            {
                return RedirectToPage("/blog");
            }

            ArticleData = await _blogArticle.GetAsync(new ContentQuery
            {
                Filter = $"data/slug/iv eq '{slug}'",
                Top = 1,
            });


            Suggestions = await _blogArticle.GetAsync(new ContentQuery
            {
                OrderBy = $"data/publisheddate/iv desc",
                Top = 3,
            });

            return Page();
        }
    }
}