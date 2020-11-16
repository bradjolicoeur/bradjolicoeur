using bradjolicoeur.core.Models.ContentModels;
using bradjolicoeur.core.Services;
using bradjolicoeur.Core.Models.ContentType;
using KenticoCloud.Delivery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Squidex.ClientLibrary;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace bradjolicoeur.web.Pages
{
    public class SiteMapModel : PageModel
    {
        private IGenerateSitemapService GenerateSitemapService { get; set; }

        private readonly IContentsClient<SitemapItem, SitemapItemData> _sitemapItem;

        public SiteMapModel(IContentsClient<SitemapItem, SitemapItemData> sitemapItem, IGenerateSitemapService generateSitemapService)
        {
            GenerateSitemapService = generateSitemapService;
            _sitemapItem = sitemapItem;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var results = await _sitemapItem.GetAsync(new ContentQuery
            {
                OrderBy = $"data/lastmodified/iv desc",
                Top = 10,
            });


            string xml = GenerateSitemapService.Generate(results.Items, HttpContext.Request.Scheme + "://" + HttpContext.Request.Host);
            return Content(xml, "text/xml", Encoding.UTF8);
        }


    }
}