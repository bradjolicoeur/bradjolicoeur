using System.Threading.Tasks;
using bradjolicoeur.core.blastcms;
using LazyCache;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace bradjolicoeur.web.Pages
{
    public class BooksModel : PageModel
    {
        private readonly IBlastCMSClient _blastcms;
        private readonly IAppCache _appCache;
        private readonly string _key;

        public BooksModel(IBlastCMSClient blastcms, IAppCache appCache, IConfiguration configuration)
        {
            _blastcms = blastcms;
            _appCache = appCache;
            _key = configuration["BlastCMSContentKey"];
        }

        public LandingPage Broadaxe { get; private set; }
        public LandingPage ConstraintLoop { get; private set; }

        public async Task OnGetAsync()
        {
            var broadaxe = _appCache.GetOrAddAsync("the-broadaxe-and-the-altar-content", async () =>
                await _blastcms.GetLandingPageBySlugAsync("the-broadaxe-and-the-altar", _key));
            var constraintLoop = _appCache.GetOrAddAsync("the-constraint-loop-content", async () =>
                await _blastcms.GetLandingPageBySlugAsync("the-constraint-loop", _key));

            await Task.WhenAll(broadaxe, constraintLoop);
            Broadaxe = await broadaxe;
            ConstraintLoop = await constraintLoop;
        }
    }
}
