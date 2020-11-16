using bradjolicoeur.core.Models.ContentModels;
using LazyCache;
using Microsoft.AspNetCore.Http;
using Squidex.ClientLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bradjolicoeur.web.Middleware
{
    public class RedirectMiddleware
    {
        private readonly IAppCache _cache;
        private RequestDelegate NextDelegate { get; set; }
        private IServiceProvider ServiceProvider { get; set; }

        private readonly IContentsClient<UrlRedirect, UrlRedirectData> _urlRedirect;

        public RedirectMiddleware(RequestDelegate nextDelegate, 
            IServiceProvider serviceProvider, 
            IContentsClient<UrlRedirect, UrlRedirectData> urlRedirect,
            IAppCache cache)
        {
            NextDelegate = nextDelegate;
            ServiceProvider = serviceProvider;
            _urlRedirect = urlRedirect;
            _cache = cache;
        }

        private async Task<IEnumerable<UrlRedirectData>> GetRedirects()
        {
            var response = await _urlRedirect.GetAsync(new ContentQuery());

            return response.Items.Select(x => x.Data);
        }

        public async Task Invoke(HttpContext httpContext)
        {
            //The path from the request
            string requestURL = httpContext.Request.Path.ToString().ToLower();

            Func<Task<IEnumerable<UrlRedirectData>>> getter = () => GetRedirects();

            //pull the full list of redirects 
            var redirects = await _cache.GetOrAddAsync("url-redirects", getter, new TimeSpan(0, 20, 0));

            //search the full list for a redirect that matches the request path
            var redirect = redirects.Where(q => q.RedirectFrom
                    .Equals(requestURL, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                        
            if (redirect != null)
            {
                //a redirect was found in the list to be executed
                httpContext.Response.Redirect(redirect.RedirectTo.ToLower(), false);
            }
            else
            {
                //invoke the next delegate in the pipeline
                await NextDelegate.Invoke(httpContext).ConfigureAwait(false);
            }

        }
    }
}
