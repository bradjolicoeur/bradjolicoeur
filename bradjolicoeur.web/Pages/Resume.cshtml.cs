using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bradjolicoeur.Core.Models.ContentType;
using KenticoCloud.Delivery;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace bradjolicoeur.web.Pages
{
    public class ResumeModel : PageModel
    {
        private IDeliveryClient DeliveryClient { get; set; }

        public ResumeModel(IDeliveryClient deliveryClient)
        {
            DeliveryClient = deliveryClient;
        }

        public ContentPage ContentPage { get; private set; }

        public async Task OnGetAsync()
        {
            var response = await DeliveryClient.GetItemsAsync<ContentPage>(
              new EqualsFilter("system.type", ContentPage.Codename),
              new EqualsFilter("system.codename", "resume_page")
              );

            ContentPage = response.Items.FirstOrDefault();
        }
    }
}