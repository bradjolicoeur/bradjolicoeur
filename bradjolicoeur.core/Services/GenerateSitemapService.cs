using bradjolicoeur.core.Models.ContentModels;
using bradjolicoeur.core.Models.Web;
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

        public string Generate(IReadOnlyList<SitemapItem> items, string pageUrl)
        {
            PageUrl = pageUrl;
            var nodes = items.Select(item => new SitemapNode(GetPageUrl(item))
            {
                LastModified = item.Data.LastModified
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

        private string GetPageUrl(SitemapItem contentItem)
        {
            return PageUrl + "/" + contentItem.Data.RelativePath;

        }
    }
}
