using bradjolicoeur.core.blastcms;
using System.Collections.Generic;

namespace bradjolicoeur.core.Services
{
    public interface IGenerateSitemapService
    {
        string Generate(IEnumerable<SitemapItem> items, string pageUrl);
    }
}