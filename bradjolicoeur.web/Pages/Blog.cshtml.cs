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
    public class BlogModel : PageModel
    {
        [FromQuery(Name = "currentpage")]
        public int CurrentPage { get; set; } = 1;

        [FromQuery(Name = "length")]
        public int PageSize { get; set; } = 10;
        public long Count { get; set; }
        public int TotalPages => (int)Math.Ceiling(decimal.Divide(Count, PageSize));

        private IDeliveryClient DeliveryClient { get; set; }

        private readonly IContentsClient<BlogArticle, BlogArticleData> _blogArticle;

        public BlogModel(IDeliveryClient deliveryClient, IContentsClient<BlogArticle, BlogArticleData> blogArtcle)
        {
            DeliveryClient = deliveryClient;
            _blogArticle = blogArtcle;
        }

        public BlogViewModel ViewModel { get; private set; }

        public async Task OnGetAsync(string id = null)
        {
            ViewModel = new BlogViewModel();

            ViewModel.ContentPage = (await DeliveryClient.GetItemsAsync<ContentPage>(
              new EqualsFilter("system.type", ContentPage.Codename),
              new EqualsFilter("system.codename", "blog_page")
              ).ConfigureAwait(false)).Items.FirstOrDefault();

            await GetBlogArticles(id);
        }

        private async Task GetBlogArticles(string tag)
        {
            var filter = string.IsNullOrEmpty(tag) ? null : $"data/blogtags/iv eq '{tag}'";

            ViewModel.BlogArticles = await _blogArticle.GetAsync(new ContentQuery
            {
                OrderBy = $"data/publisheddate/iv desc",
                Filter = filter,
                Skip = ((CurrentPage - 1) * PageSize),
                Top = PageSize,
            });

            Count = ViewModel.BlogArticles.Total;

            //var param = new List<IQueryParameter> {
            //    new EqualsFilter("system.type", BlogArticle.Codename),
            //    new OrderParameter("elements." + BlogArticle.PublishedDateCodename, SortOrder.Descending)
            //};

            //if (!string.IsNullOrEmpty(tag))
            //{
            //    param.Add(new ContainsFilter("elements." + BlogArticle.TagsCodename, tag));
            //}

            //return (await DeliveryClient.GetItemsAsync<BlogArticle>(
            //        param.ToArray()
            //        ).ConfigureAwait(false)).Items.ToArray();
        }
    }
}