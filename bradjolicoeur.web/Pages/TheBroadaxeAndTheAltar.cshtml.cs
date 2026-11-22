using System.Threading.Tasks;
using bradjolicoeur.core.blastcms;
using LazyCache;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace bradjolicoeur.web.Pages
{
    public class TheBroadaxeAndTheAltarModel : PageModel
    {
        private readonly IBlastCMSClient _blastcms;
        private readonly IAppCache _appCache;
        private readonly string _key;

        public TheBroadaxeAndTheAltarModel(IBlastCMSClient blastcms, IAppCache appCache, IConfiguration configuration)
        {
            _blastcms = blastcms;
            _appCache = appCache;
            _key = configuration["BlastCMSContentKey"];
        }

        public LandingPage PageData { get; set; }

        public async Task OnGetAsync()
        {
            PageData = await _appCache.GetOrAddAsync("the-broadaxe-and-the-altar-content", async () =>
                await _blastcms.GetLandingPageBySlugAsync("the-broadaxe-and-the-altar", _key));
        }
    }
}
