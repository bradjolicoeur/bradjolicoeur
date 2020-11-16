using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bradjolicoeur.core.Models.ContentModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Squidex.ClientLibrary;

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

        private readonly IContentsClient<ReadingFeed, ReadingFeedData> _readingFeed;

        public ReadingModel(IContentsClient<ReadingFeed, ReadingFeedData> readingFeed)
        {
            _readingFeed = readingFeed;
        }

        public ContentsResult<ReadingFeed, ReadingFeedData> content;

        public bool NextPage => (CurrentPage * PageSize) <= content.Total;

        public bool PreviousPage => (CurrentPage > 1);

        public async Task OnGetAsync()
        {
            CurrentPage = CurrentPage < 1 ? 1 : CurrentPage;

            content = await _readingFeed.GetAsync(new ContentQuery
            {
                OrderBy = $"data/dateposted/iv desc",
                Skip = ((CurrentPage - 1) * PageSize),
                Top = PageSize,
            }) ;

            Count = content.Total;
        }
    }
}
