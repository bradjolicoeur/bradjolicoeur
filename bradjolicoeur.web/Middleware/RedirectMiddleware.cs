using bradjolicoeur.core.blastcms;
using LazyCache;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bradjolicoeur.web.Middleware
{
    public class RedirectMiddleware
    {
        private readonly IAppCache _cache;
        private readonly object _key;

        private RequestDelegate NextDelegate { get; set; }

        private readonly IBlastCMSClient _blastcms;

        public RedirectMiddleware(RequestDelegate nextDelegate, 
            IBlastCMSClient blastcms,
            IAppCache cache,
            IConfiguration configuration)
        {
            NextDelegate = nextDelegate;
            _blastcms = blastcms;
            _cache = cache;
            _key = configuration["BlastCMSContentKey"];
        }

        private async Task<IEnumerable<UrlRedirect>> GetRedirects()
        {
            var response = await _blastcms.GetUrlRedirectsAsync(0,1000,1, _key);

            return response.Data;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            //The path from the request
            string requestURL = httpContext.Request.Path.ToString().ToLower();

            Func<Task<IEnumerable<UrlRedirect>>> getter = () => GetRedirects();

            //pull the full list of redirects 
            var redirects = await _cache.GetOrAddAsync("url-redirects", getter, new TimeSpan(0, 20, 0));

            //search the full list for a redirect that matches the request path
            var redirect = redirects.Where(q => q.RedirectFrom
                    .Equals(requestURL, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                        
            if (redirect != null)
            {
                //a redirect was found in the list to be executed
                httpContext.Response.Redirect(redirect.RedirectTo.ToLower(), redirect.Permanent);
            }
            else
            {
                //invoke the next delegate in the pipeline
                await NextDelegate.Invoke(httpContext);
            }

        }
    }
}
