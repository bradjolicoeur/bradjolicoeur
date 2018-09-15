using System.Collections.Generic;
using KenticoCloud.Delivery;

namespace bradjolicoeur.core.Services
{
    public interface IGenerateSitemapService
    {
        string Generate(IReadOnlyList<ContentItem> items, string pageUrl);
    }
}