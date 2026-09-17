using System.Threading.Tasks;
using bradjolicoeur.core.blastcms;
using LazyCache;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace bradjolicoeur.web.Pages
{
    public class TheConstraintLoopModel : PageModel
    {
        private readonly IBlastCMSClient _blastcms;
        private readonly IAppCache _appCache;
        private readonly string _key;

        public TheConstraintLoopModel(IBlastCMSClient blastcms, IAppCache appCache, IConfiguration configuration)
        {
            _blastcms = blastcms;
            _appCache = appCache;
            _key = configuration["BlastCMSContentKey"];
        }

        public LandingPage PageData { get; set; }

        public async Task OnGetAsync()
        {
            PageData = await _appCache.GetOrAddAsync("the-constraint-loop-content", async () =>
                await _blastcms.GetLandingPageBySlugAsync("the-constraint-loop", _key));
        }
    }
}
