using bradjolicoeur.core.Models.ContentModels;
using Squidex.ClientLibrary;
using System.Threading.Tasks;

namespace bradjolicoeur.web.Services
{
    public interface ISuggestionArticlesService
    {
        Task<ContentsResult<BlogArticle, BlogArticleData>> GetSuggestions();
    }
}