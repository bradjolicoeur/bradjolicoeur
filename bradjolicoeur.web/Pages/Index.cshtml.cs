using System;
using System.Linq;
using System.Threading.Tasks;
using bradjolicoeur.core.Models.ContentModels;
using bradjolicoeur.web.Services;
using LazyCache;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Squidex.ClientLibrary;

namespace bradjolicoeur.web.Pages
{
    public class IndexModel : PageModel
    {

        private readonly IContentsClient<BlogArticle, BlogArticleData> _blogArticle;
        private readonly IContentsClient<HomePage, HomePageData> _homePage;
        private readonly ISquidexClientManager _squidex;
        private readonly IAppCache _appCache;
        private readonly ISuggestionArticlesService _suggestionService;

        public IndexModel(IContentsClient<BlogArticle, BlogArticleData> blogArtcle, IContentsClient<HomePage, HomePageData> homePage, 
            ISquidexClientManager squidex, IAppCache appCache, ISuggestionArticlesService suggestionService)
        {
            _blogArticle = blogArtcle;
            _homePage = homePage;
            _squidex = squidex;
            _appCache = appCache;
            _suggestionService = suggestionService;
        }

        public ContentsResult<HomePage, HomePageData> HomePageData { get; set; }
        public HomePage HomePage { get => HomePageData?.Items?.FirstOrDefault(); }
        public ContentsResult<BlogArticle, BlogArticleData> BlogArticles { get; set; }
        public string ImageUrl { get => _squidex.GenerateImageUrl(HomePage.Data.ProfileImage.FirstOrDefault()); }

        public async Task OnGetAsync()
        {
            Func<Task<ContentResults>> showObjectFactory = () => GetContent();

            var content = await _appCache.GetOrAddAsync("home_page_content", showObjectFactory);

            HomePageData = content.HomePageData;
            BlogArticles = content.BlogArticles;

        }

        private class ContentResults
        {
            public ContentsResult<BlogArticle, BlogArticleData> BlogArticles { get; set; }
            public ContentsResult<HomePage, HomePageData> HomePageData { get; set; }
        }

        private async Task<ContentResults> GetContent()
        {
            var result = new ContentResults();

            result.HomePageData = await _homePage.GetAsync(new ContentQuery
            {
                Filter = $"id eq 'bdb9893d-4bee-44bb-bd81-73ca73dda795'"
            });


            result.BlogArticles = await _suggestionService.GetSuggestions();

            return result;
        }
    }
}
