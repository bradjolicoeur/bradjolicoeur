using System.Collections.Generic;
using bradjolicoeur.core.Models.ContentModels;

namespace bradjolicoeur.core.Services
{
    public interface IGenerateSitemapService
    {
        string Generate(IReadOnlyList<SitemapItem> items, string pageUrl);
    }
}