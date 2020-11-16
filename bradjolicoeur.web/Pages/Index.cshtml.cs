using System.Linq;
using System.Threading.Tasks;
using bradjolicoeur.core.Models.ContentModels;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Squidex.ClientLibrary;

namespace bradjolicoeur.web.Pages
{
    public class IndexModel : PageModel
    {

        private readonly IContentsClient<BlogArticle, BlogArticleData> _blogArticle;
        private readonly IContentsClient<HomePage, HomePageData> _homePage;
        private readonly ISquidexClientManager _squidex;

        public IndexModel(IContentsClient<BlogArticle, BlogArticleData> blogArtcle, IContentsClient<HomePage, HomePageData> homePage, ISquidexClientManager squidex)
        {
            _blogArticle = blogArtcle;
            _homePage = homePage;
            _squidex = squidex;
        }

        public ContentsResult<HomePage, HomePageData> HomePageData { get; set; }
        public HomePage HomePage { get => HomePageData?.Items?.FirstOrDefault(); }
        public ContentsResult<BlogArticle, BlogArticleData> BlogArticles { get; set; }
        public string ImageUrl { get => _squidex.GenerateImageUrl(HomePage.Data.ProfileImage.FirstOrDefault()); }

        public async Task OnGetAsync()
        {

            HomePageData = await _homePage.GetAsync(new ContentQuery
            {
                Filter = $"id eq 'bdb9893d-4bee-44bb-bd81-73ca73dda795'"
            });


            BlogArticles = await _blogArticle.GetAsync(new ContentQuery
            {
                OrderBy = $"data/publisheddate/iv desc",
                Top = 3,
            });

        }
    }
}
