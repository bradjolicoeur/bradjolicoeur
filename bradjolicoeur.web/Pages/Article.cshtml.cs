using System.Linq;
using System.Threading.Tasks;
using bradjolicoeur.Core.Models.ContentType;
using bradjolicoeur.web.ViewModels;
using KenticoCloud.Delivery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace bradjolicoeur.web.Pages
{
    public class ArticleModel : PageModel
    {
        private IDeliveryClient DeliveryClient { get; set; }

        public ArticleModel(IDeliveryClient deliveryClient)
        {
            DeliveryClient = deliveryClient;
        }

        public ArticleViewModel ViewModel { get; private set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return RedirectToPage("/blog");
            }

            ViewModel = new ArticleViewModel();

            ViewModel.Article = (await DeliveryClient.GetItemsAsync<BlogArticle>(
              new EqualsFilter("system.type", BlogArticle.Codename),
              new EqualsFilter("elements." + BlogArticle.RouteCodename, id),
               new DepthParameter(2)
              ).ConfigureAwait(false)).Items.FirstOrDefault();

            ViewModel.Suggestions = (await DeliveryClient.GetItemsAsync<BlogArticle>(
              new LimitParameter(4),
                new OrderParameter("elements." + BlogArticle.PublishedDateCodename, SortOrder.Descending)
              ).ConfigureAwait(false)).Items.ToArray().Where(q => q.Route.ToLower() != id.ToLower()).Take(3);

            return Page();
        }
    }
}