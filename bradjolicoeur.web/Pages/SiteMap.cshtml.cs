using bradjolicoeur.core.Services;
using bradjolicoeur.Core.Models.ContentType;
using KenticoCloud.Delivery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace bradjolicoeur.web.Pages
{
    public class SiteMapModel : PageModel
    {
        private IDeliveryClient DClient { get; set; }
        private IGenerateSitemapService GenerateSitemapService { get; set; }

        public SiteMapModel(IDeliveryClient dcFactory, IGenerateSitemapService generateSitemapService)
        {
            DClient = dcFactory;
            GenerateSitemapService = generateSitemapService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var parameters = new List<IQueryParameter>
            {
                new DepthParameter(0),
                new InFilter("system.type", ContentPage.Codename),
            };

            var response = await DClient.GetItemsAsync(parameters).ConfigureAwait(false);


            string xml = GenerateSitemapService.Generate(response.Items, HttpContext.Request.Scheme + "://" + HttpContext.Request.Host);
            return Content(xml, "text/xml", Encoding.UTF8);
        }


    }
}