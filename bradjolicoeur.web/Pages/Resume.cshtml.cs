using System;
using System.Linq;
using System.Threading.Tasks;
using bradjolicoeur.core.blastcms;
using LazyCache;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Squidex.ClientLibrary;

namespace bradjolicoeur.web.Pages
{
    public class ResumeModel : PageModel
    {
        private readonly IBlastCMSClient _blastcms;
        private readonly IConfiguration _configuration;
        private readonly IAppCache _appCache;
        private readonly string _key;

        public ResumeModel(IBlastCMSClient blastcms, IAppCache appCache, IConfiguration configuration)
        {
            _blastcms = blastcms;
            _configuration = configuration;
            _appCache = appCache;
            _key = _configuration["BlastCMSContentKey"];
        }

        public LandingPage Resume { get; set; }


        public async Task OnGet()
        {
            Resume = await _appCache.GetOrAddAsync("resume-content", async () =>
            {
                return await _blastcms.GetLandingPage2Async("resume-page", _key);
            });
        }

       
    }
}