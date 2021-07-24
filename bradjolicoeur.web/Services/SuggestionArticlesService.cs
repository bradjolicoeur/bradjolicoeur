using bradjolicoeur.core.Models.ContentModels;
using LazyCache;
using Squidex.ClientLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bradjolicoeur.web.Services
{
    public class SuggestionArticlesService : ISuggestionArticlesService
    {
        private readonly IContentsClient<BlogArticle, BlogArticleData> _blogArticle;
        private readonly IAppCache _appCache;

        public SuggestionArticlesService(IContentsClient<BlogArticle, BlogArticleData> blogArtcle, IAppCache appCache)
        {
            _blogArticle = blogArtcle;
            _appCache = appCache;
        }

        public async Task<ContentsResult<BlogArticle, BlogArticleData>> GetSuggestions()
        {
            return await _appCache.GetOrAddAsync("blog-article-suggestions", () => GetContents());
        }

        private async Task<ContentsResult<BlogArticle, BlogArticleData>> GetContents()
        {
            return await _blogArticle.GetAsync(new ContentQuery
            {
                OrderBy = $"data/publisheddate/iv desc",
                Top = 3,
            });
        }
    }
}
