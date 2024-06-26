﻿using bradjolicoeur.core.blastcms;
using LazyCache;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace bradjolicoeur.web.Services
{
    public class SuggestionArticlesService : ISuggestionArticlesService
    {
        private readonly IBlastCMSClient _blastcms;
        private readonly IAppCache _appCache;
        private readonly string _key;

        public SuggestionArticlesService(IBlastCMSClient blastcms, IAppCache appCache, IConfiguration configuration)
        {
            _blastcms = blastcms;
            _appCache = appCache;
            _key = configuration["BlastCMSContentKey"];
        }

        public async Task<IEnumerable<BlogArticle>> GetSuggestions()
        {
            return await _appCache.GetOrAddAsync("blog-article-suggestions", () => GetContents());
        }

        private async Task<IEnumerable<BlogArticle>> GetContents()
        {

            var BlogArticles = await _blastcms.GetBlogArticlesAsync(0, 4, 1, null, null, _key);
            return BlogArticles.Data;
        }
    }
}
