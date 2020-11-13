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
        private readonly IContentsClient<ReadingFeed, ReadingFeedData> _readingFeed;

        public ReadingModel(IContentsClient<ReadingFeed, ReadingFeedData> readingFeed)
        {
            _readingFeed = readingFeed;
        }

        public ContentsResult<ReadingFeed, ReadingFeedData> content;

        public async Task OnGetAsync()
        {
            content = await _readingFeed.GetAsync(new ContentQuery
            {
                OrderBy = $"data/dateposted/iv desc"
            });
        }
    }
}
