using bradjolicoeur.core.Models.Web;
using bradjolicoeur.Core.Models.ContentType;
using KenticoCloud.Delivery;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

namespace bradjolicoeur.core.Services
{
    public class GenerateSitemapService : IGenerateSitemapService
    {

        private string PageUrl { get; set; }

        public string Generate(IReadOnlyList<ContentItem> items, string pageUrl)
        {
            PageUrl = pageUrl;
            var nodes = items.Select(item => new SitemapNode(GetPageUrl(item))
            {
                LastModified = item.System.LastModified
            }).ToList();

            return GetSitemapDocument(nodes);
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
            string pageName = PageUrl;
            switch (contentItem.System.Type)
            {
                case "content_page":
                    var item = contentItem.CastTo<ContentPage>();
                    pageName += "/" + ((item.Route.ToLower() != "home")? item.Route : "");
                    break;

                case "blog_article":
                    var ei = contentItem.CastTo<BlogArticle>();
                    pageName += "/article/" + ei.Route;
                    break;

                default:
                    break;
            }

            return pageName;
        }
    }
}
