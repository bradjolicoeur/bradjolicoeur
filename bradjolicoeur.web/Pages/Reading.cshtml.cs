using System;
using System.Threading.Tasks;
using bradjolicoeur.core.blastcms;
using LazyCache;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace bradjolicoeur.web.Pages
{
    public class ReadingModel : PageModel
    {
        [FromQuery(Name = "currentpage")]
        public int CurrentPage { get; set; } = 1;

        [FromQuery(Name = "length")]
        public int PageSize { get; set; } = 10;
        public long Count { get; set; }
        public int TotalPages => (int)Math.Ceiling(decimal.Divide(Count, PageSize));

        [BindProperty(SupportsGet = true)]
        [FromQuery(Name = "search")]
        public string Search { get; set; } = null;

        private readonly IBlastCMSClient _blastcms;
        private readonly IConfiguration _configuration;
        private readonly IAppCache _appCache;
        private readonly string _key;

        public ReadingModel(IBlastCMSClient blastcms, IAppCache appCache, IConfiguration configuration)
        {
            _blastcms = blastcms;
            _configuration = configuration;
            _appCache = appCache;
            _key = _configuration["BlastCMSContentKey"];
        }

        public FeedArticleIPagedData content;

        public bool NextPage => (CurrentPage * PageSize) <= Count;

        public bool PreviousPage => (CurrentPage > 1);

        public async Task OnGetAsync()
        {
            CurrentPage = CurrentPage < 1 ? 1 : CurrentPage;

            content = await GetContent(Search);

            Count = content.Count;
        }

        private async Task<FeedArticleIPagedData> GetContent(string search)
        {
            return await _blastcms.GetFeedArticlesAsync(((CurrentPage - 1) * PageSize), PageSize, CurrentPage, search,  _key);
        }
    }
}
