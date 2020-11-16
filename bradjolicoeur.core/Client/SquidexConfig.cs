using bradjolicoeur.core.Models.ContentModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Squidex.ClientLibrary;
using Squidex.ClientLibrary.Management;
using System;
using System.Collections.Generic;
using System.Text;

namespace bradjolicoeur.core.Client
{
    public static class SquidexConfig
    {
        public static void AddSquidexServices(this IServiceCollection services, IConfiguration configuration)
        {
            var clientManager =
                new SquidexClientManager(
                    new SquidexOptions
                    {
                        AppName = "brad-jolicoeur-blog",
                        ClientId = configuration["SquidexClientId"] ,
                        ClientSecret = configuration["SquidexClientSecret"],
                        Url = "https://cloud.squidex.io"
                    });

            services.AddSingleton<ISquidexClientManager>(clientManager);

            services.AddSingleton<IAssetsClient>(c =>
                c.GetRequiredService<SquidexClientManager>()
                    .CreateAssetsClient());

            services.AddSingleton(clientManager.CreateContentsClient<ReadingFeed, ReadingFeedData>("reading-feed"));
            services.AddSingleton(clientManager.CreateContentsClient<BlogArticle, BlogArticleData>("blog-article"));
            services.AddSingleton(clientManager.CreateContentsClient<Resume, ResumeData>("resume"));
            services.AddSingleton(clientManager.CreateContentsClient<HomePage, HomePageData>("home-page"));
            services.AddSingleton(clientManager.CreateContentsClient<SitemapItem, SitemapItemData>("sitemap-item"));
            services.AddSingleton(clientManager.CreateContentsClient<UrlRedirect, UrlRedirectData>("url-redirect"));
        }
    }
}
