using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bradjolicoeur.core.Models.ContentModels;
using bradjolicoeur.Core.Models.ContentType;
using bradjolicoeur.web.ViewModels;
using KenticoCloud.Delivery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Squidex.ClientLibrary;

namespace bradjolicoeur.web.Pages
{
    public class IndexModel : PageModel
    {
        private IDeliveryClient DeliveryClient { get; set; }

        private readonly IContentsClient<BlogArticle, BlogArticleData> _blogArticle;

        public IndexModel(IDeliveryClient deliveryClient, IContentsClient<BlogArticle, BlogArticleData> blogArtcle)
        {
            DeliveryClient = deliveryClient;
            _blogArticle = blogArtcle;
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

            ViewModel.BlogArticles = await _blogArticle.GetAsync(new ContentQuery
            {
                OrderBy = $"data/publisheddate/iv desc",
                Top = 3,
            });

        }
    }
}
