using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using bradjolicoeur.core.blastcms;
using LazyCache;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace bradjolicoeur.web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IAppCache _appCache;
        private readonly IBlastCMSClient _blastcms;
        private readonly string _key;

        public IndexModel(IBlastCMSClient blastcms, IAppCache appCache, IConfiguration configuration)
        {
            _appCache = appCache;
            _blastcms = blastcms;
            _key = configuration["BlastCMSContentKey"];
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

            var homePageTask = _blastcms.GetLandingPageBySlugAsync("home-page", _key);
            var article1Task = _blastcms.GetBlogArticleBySlugAsync("copilot-squad-dotnet-web-apps", _key);
            var article2Task = _blastcms.GetBlogArticleBySlugAsync("wolverine-vs-ai-agents-messaging-framework", _key);
            var article3Task = _blastcms.GetBlogArticleBySlugAsync("disposable-code-architects-perspective", _key);

            await Task.WhenAll(homePageTask, article1Task, article2Task, article3Task);

            result.HomePage = await homePageTask;
            result.BlogArticles = new List<BlogArticle>
            {
                await article1Task,
                await article2Task,
                await article3Task
            };

            return result;
        }
    }
}
