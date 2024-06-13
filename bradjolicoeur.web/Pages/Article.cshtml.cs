using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bradjolicoeur.core.blastcms;
using bradjolicoeur.web.Services;
using LazyCache;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace bradjolicoeur.web.Pages
{
    public class ArticleModel : PageModel
    {

        private readonly IAppCache _appCache;
        private readonly ISuggestionArticlesService _suggestionService;
        private readonly IBlastCMSClient _blastcms;
        private readonly string _key;

        public ArticleModel(IBlastCMSClient blastcms, IAppCache appCache, IConfiguration configuration, ISuggestionArticlesService suggestionService)
        {
            _appCache = appCache;
            _suggestionService = suggestionService;
            _blastcms = blastcms;
            _key = configuration["BlastCMSContentKey"];
        }


        public BlogArticle Article { get; set; }

        public ImageFile ImageUrl { get => Article.Image; }

        public IEnumerable<BlogArticle> Suggestions { get; set; }

        public async Task<IActionResult> OnGet(string slug)
        {
            if (slug == null)
            {
                return RedirectToPage("/blog");
            }

            Article = await _appCache.GetOrAddAsync($"blogarticle-{slug.ToLower()}", () => GetContent(slug));


            Suggestions = await FilterSuggestions();

            return Page();
        }

        private async Task<IEnumerable<BlogArticle>> FilterSuggestions()
        {
            var suggestions = await _suggestionService.GetSuggestions();

            return suggestions.Where(q => q.Slug != Article.Slug).Take(3);
        }

        private async Task<BlogArticle> GetContent(string slug)
        {
            return await _blastcms.GetBlogArticleBySlugAsync(slug, _key);
        }
    }
}