
using bradjolicoeur.core.blastcms;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace bradjolicoeur.web.Services
{
    public interface ISuggestionArticlesService
    {
        Task<IEnumerable<BlogArticle>> GetSuggestions();
    }
}