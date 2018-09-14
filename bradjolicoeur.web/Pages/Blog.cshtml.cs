using System.Linq;
using System.Threading.Tasks;
using bradjolicoeur.Core.Models.ContentType;
using bradjolicoeur.web.ViewModels;
using KenticoCloud.Delivery;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace bradjolicoeur.web.Pages
{
    public class BlogModel : PageModel
    {
        private IDeliveryClient DeliveryClient { get; set; }

        public BlogModel(IDeliveryClient deliveryClient)
        {
            DeliveryClient = deliveryClient;
        }

        public BlogViewModel ViewModel { get; private set; }

        public async Task OnGetAsync()
        {
            var viewModel = new BlogViewModel();

            viewModel.ContentPage = (await DeliveryClient.GetItemsAsync<ContentPage>(
              new EqualsFilter("system.type", ContentPage.Codename),
              new EqualsFilter("system.codename", "blog_page")
              ).ConfigureAwait(false)).Items.FirstOrDefault();

            viewModel.BlogArticles = (await DeliveryClient.GetItemsAsync<BlogArticle>(
              new EqualsFilter("system.type", BlogArticle.Codename),
              new OrderParameter("elements." + BlogArticle.PublishedDateCodename, SortOrder.Descending)
              ).ConfigureAwait(false)).Items.ToArray();

            ViewModel = viewModel;
        }
    }
}