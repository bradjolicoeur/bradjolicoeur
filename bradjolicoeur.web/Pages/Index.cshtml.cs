using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bradjolicoeur.Core.Models.ContentType;
using bradjolicoeur.web.ViewModels;
using KenticoCloud.Delivery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace bradjolicoeur.web.Pages
{
    public class IndexModel : PageModel
    {
        private IDeliveryClient DeliveryClient { get; set; }

        public IndexModel(IDeliveryClient deliveryClient)
        {
            DeliveryClient = deliveryClient;
        }

        public HomeViewModel ViewModel { get; private set; }

        public async Task OnGetAsync()
        {

            ViewModel = new HomeViewModel();
                
            ViewModel.ContentPage  = (await DeliveryClient.GetItemsAsync<ContentPage>(
              new EqualsFilter("system.type", ContentPage.Codename),
              new EqualsFilter("system.codename", "home_page"),
               new DepthParameter(2)
              ).ConfigureAwait(false)).Items.FirstOrDefault();

            ViewModel.RecentArticles = (await DeliveryClient.GetItemsAsync<BlogArticle>(
              new LimitParameter(4),
                new OrderParameter("elements." + BlogArticle.PublishedDateCodename, SortOrder.Descending)
              ).ConfigureAwait(false)).Items.ToArray();

        }
    }
}
