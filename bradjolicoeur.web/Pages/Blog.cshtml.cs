using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using bradjolicoeur.core.blastcms;

using LazyCache;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;


namespace bradjolicoeur.web.Pages
{
    public class BlogModel : PageModel
    {
        [FromQuery(Name = "currentpage")]
        public int CurrentPage { get; set; } = 1;

        [FromQuery(Name = "length")]
        public int PageSize { get; set; } = 10;
        public long Count { get; set; }
        public int TotalPages => (int)Math.Ceiling(decimal.Divide(Count, PageSize));
        public bool NextPage => (CurrentPage * PageSize) <= Count;
        public bool PreviousPage => (CurrentPage > 1);

        private readonly IBlastCMSClient _blastcms;
        private readonly IConfiguration _configuration;
        private readonly IAppCache _appCache;
        private readonly string _key;

        public BlogModel(IBlastCMSClient blastcms, IAppCache appCache, IConfiguration configuration)
        {
            _blastcms = blastcms;
            _configuration = configuration;
            _appCache = appCache;
            _key = _configuration["BlastCMSContentKey"];
        }

        public BlogArticleIPagedData BlogArticles { get; set; }

        public async Task OnGetAsync(string id = null)
        {

            await GetBlogArticles(id);
        }

        private async Task GetBlogArticles(string tag)
        {
            CurrentPage = CurrentPage < 1 ? 1 : CurrentPage;


            BlogArticles = await _blastcms.GetBlogArticlesAsync(((CurrentPage - 1) * PageSize), PageSize, CurrentPage, null, tag, _key);

            //BlogArticles = await _appCache.GetOrAddAsync(JsonConvert.SerializeObject(contentQuery), () => GetContent(contentQuery));

            Count = BlogArticles.Count;

        }


    }
}