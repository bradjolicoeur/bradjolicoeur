using bradjolicoeur.core.Models.Web;
using bradjolicoeur.Core.Models.ContentType;
using KenticoCloud.Delivery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace bradjolicoeur.web.Pages
{
    public class SiteMapModel : PageModel
    {
        private IDeliveryClient DClient { get; set; }

        public SiteMapModel(IDeliveryClient dcFactory)
        {
            DClient = dcFactory;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var parameters = new List<IQueryParameter>
            {
                new DepthParameter(0),
                new InFilter("system.type", ContentPage.Codename, BlogArticle.Codename),
            };

            var response = await DClient.GetItemsAsync(parameters).ConfigureAwait(false);

            var nodes = response.Items.Select(item => new SitemapNode(GetPageUrl(item))
            {
                LastModified = item.System.LastModified
            })
                .ToList();

            string xml = GetSitemapDocument(nodes);
            return Content(xml, "text/xml", Encoding.UTF8);
        }

        private string GetSitemapDocument(IEnumerable<SitemapNode> sitemapNodes)
        {
            XNamespace xmlns = "http://www.sitemaps.org/schemas/sitemap/0.9";
            XElement root = new XElement(xmlns + "urlset");

            foreach (SitemapNode sitemapNode in sitemapNodes)
            {
                XElement urlElement = new XElement(
                    xmlns + "url",
                    new XElement(xmlns + "loc", Uri.EscapeUriString(sitemapNode.Url)),
                    sitemapNode.LastModified == null ? null : new XElement(
                        xmlns + "lastmod",
                        sitemapNode.LastModified.Value.ToLocalTime().ToString("yyyy-MM-ddTHH:mm:sszzz")),
                    sitemapNode.Frequency == null ? null : new XElement(
                        xmlns + "changefreq",
                        sitemapNode.Frequency.Value.ToString().ToLowerInvariant()),
                    sitemapNode.Priority == null ? null : new XElement(
                        xmlns + "priority",
                        sitemapNode.Priority.Value.ToString("F1", CultureInfo.InvariantCulture)));
                root.Add(urlElement);
            }

            XDocument document = new XDocument(root);
            return document.ToString();
        }

        private string GetPageUrl(ContentItem contentItem)
        {
            var system = contentItem.System;
            string pageName = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host;
            switch (contentItem.System.Type)
            {
                case "content_page":
                    var item = contentItem.CastTo<ContentPage>();
                    pageName += "/" + item.Route;
                    break;

                case "blog_article":
                    var ei = contentItem.CastTo<BlogArticle>();
                    pageName += "/blog/article/" + ei.Route;
                    break;

                default:
                    break;
            }

            return pageName;
        }
    }
}