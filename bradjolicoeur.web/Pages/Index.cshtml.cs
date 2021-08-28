using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bradjolicoeur.core.blastcms;
using bradjolicoeur.web.Services;
using LazyCache;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Squidex.ClientLibrary;

namespace bradjolicoeur.web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IAppCache _appCache;
        private readonly IBlastCMSClient _blastcms;
        private readonly string _key;
        private readonly ISuggestionArticlesService _suggestionService;

        public IndexModel(IBlastCMSClient blastcms, IAppCache appCache, ISuggestionArticlesService suggestionService, IConfiguration configuration)
        {
            _appCache = appCache;
            _blastcms = blastcms;
            _key = configuration["BlastCMSContentKey"];
            _suggestionService = suggestionService;
        }

        public LandingPage HomePage { get; set; }
        public IEnumerable<BlogArticle> BlogArticles { get; set; }
        public string ImageUrl { get => HomePage.HeroImageUrl; }

        public async Task OnGetAsync()
        {
            Func<Task<ContentResults>> showObjectFactory = () => GetContent();

            var content = await _appCache.GetOrAddAsync("home_page_content", showObjectFactory);

            HomePage = content.HomePage;
            BlogArticles = content.BlogArticles;

        }

        private class ContentResults
        {
            public IEnumerable<BlogArticle> BlogArticles { get; set; }
            public LandingPage HomePage { get; set; }
        }

        private async Task<ContentResults> GetContent()
        {
            var result = new ContentResults();

            result.HomePage = await _blastcms.GetLandingPage2Async("home-page", _key);

            result.BlogArticles = await _suggestionService.GetSuggestions();

            return result;
        }
    }
}
