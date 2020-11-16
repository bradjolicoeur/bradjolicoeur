using System.Collections.Generic;
using bradjolicoeur.core.Models.ContentModels;
using KenticoCloud.Delivery;

namespace bradjolicoeur.core.Services
{
    public interface IGenerateSitemapService
    {
        string Generate(IReadOnlyList<SitemapItem> items, string pageUrl);
    }
}