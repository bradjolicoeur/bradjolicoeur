using System.Linq;
using System.Threading.Tasks;
using bradjolicoeur.core.Models.ContentModels;
using bradjolicoeur.web.Services;
using LazyCache;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Squidex.ClientLibrary;

namespace bradjolicoeur.web.Pages
{
    public class ArticleModel : PageModel
    {
        private readonly IContentsClient<BlogArticle, BlogArticleData> _blogArticle;

        public ISquidexClientManager _squidex { get; }

        private readonly IAppCache _appCache;
        private readonly ISuggestionArticlesService _suggestionService;

        public ArticleModel(IContentsClient<BlogArticle, BlogArticleData> blogArtcle, ISquidexClientManager squidex, IAppCache appCache, ISuggestionArticlesService suggestionService)
        {
            _blogArticle = blogArtcle;
            _squidex = squidex;
            _appCache = appCache;
            _suggestionService = suggestionService;
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

            ArticleData = await _appCache.GetOrAddAsync($"blogarticle-{slug.ToLower()}", () => GetContent(slug));


            Suggestions = await _suggestionService.GetSuggestions();

            return Page();
        }

        private async Task<ContentsResult<BlogArticle, BlogArticleData>> GetContent(string slug)
        {
            return await _blogArticle.GetAsync(new ContentQuery
            {
                Filter = $"data/slug/iv eq '{slug}'",
                Top = 1,
            });
        }
    }
}