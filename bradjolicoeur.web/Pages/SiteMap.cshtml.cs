using bradjolicoeur.core.blastcms;
using bradjolicoeur.core.Services;
using LazyCache;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Threading.Tasks;

namespace bradjolicoeur.web.Pages
{
    public class SiteMapModel : PageModel
    {
        private readonly IGenerateSitemapService _generateSitemapService;

        private readonly IBlastCMSClient _blastcms;
        private readonly IConfiguration _configuration;
        private readonly IAppCache _appCache;
        private readonly string _key;

        public SiteMapModel(IBlastCMSClient blastcms, IAppCache appCache, IConfiguration configuration, IGenerateSitemapService generateSitemapService)
        {
            _blastcms = blastcms;
            _configuration = configuration;
            _appCache = appCache;
            _key = _configuration["BlastCMSContentKey"];
            _generateSitemapService = generateSitemapService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var results = await _appCache.GetOrAddAsync("sitemap-content", async () =>
            {
                return await _blastcms.GetSitemapItemsAsync(0, 100, 1, _key);
            });

            var articles = await _appCache.GetOrAddAsync("sitemap-articles", async () =>
            {
                return await _blastcms.GetBlogArticlesAsync(0, 200, 1, null, null, _key);
            });

            string xml = _generateSitemapService.Generate(results.Data, "https://" + HttpContext.Request.Host, articles.Data);
            return Content(xml, "text/xml", Encoding.UTF8);
        }


    }
}