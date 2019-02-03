using bradjolicoeur.Core.Models.ContentType;
using KenticoCloud.Delivery;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bradjolicoeur.web.Middleware
{
    public class RedirectMiddleware
    {
        private RequestDelegate NextDelegate { get; set; }
        private IServiceProvider ServiceProvider { get; set; }
        private IDeliveryClient DeliveryClient { get; set; }

        public RedirectMiddleware(RequestDelegate nextDelegate, 
            IServiceProvider serviceProvider, IDeliveryClient deliveryClient)
        {
            NextDelegate = nextDelegate;
            ServiceProvider = serviceProvider;
            DeliveryClient = deliveryClient;
        }

        private async Task<IEnumerable<UrlRedirect>> GetRedirects()
        {
            var response = await DeliveryClient.GetItemsAsync<UrlRedirect>(
               new EqualsFilter("system.type", UrlRedirect.Codename)
              ).ConfigureAwait(false);

            return response.Items;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            //The path from the request
            string requestURL = httpContext.Request.Path.ToString().ToLower();

            //pull the full list of redirects 
            var redirects = await GetRedirects();

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
