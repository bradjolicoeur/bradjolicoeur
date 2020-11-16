using System;
using System.Linq;
using System.Threading.Tasks;
using bradjolicoeur.core.Models.ContentModels;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Squidex.ClientLibrary;

namespace bradjolicoeur.web.Pages
{
    public class ResumeModel : PageModel
    {
        private readonly IContentsClient<Resume, ResumeData> _resume;

        public ResumeModel(IContentsClient<Resume, ResumeData> resume)
        {
            _resume = resume;
        }

        public Resume Resume { get => content?.Items?.FirstOrDefault(); }

        public ContentsResult<Resume, ResumeData> content { get; set; }

        public async Task OnGet()
        {
            content = await _resume.GetAsync(new ContentQuery
            {
                Filter = $"id eq '603b2d8a-c44f-4be1-be30-b61a205dbbf0'"
            });

        }
    }
}