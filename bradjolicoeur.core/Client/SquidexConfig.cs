using bradjolicoeur.core.Models.ContentModels;
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
        public static void AddSquidexServices(this IServiceCollection services)
        {
            var clientManager =
                new SquidexClientManager(
                    new SquidexOptions
                    {
                        AppName = "brad-jolicoeur-blog",
                        ClientId = "brad-jolicoeur-blog:read-only",
                        ClientSecret = "1cq5l90ferjma3bukon11hhwgzbxhnio8fa1oxkzxjax",
                        Url = "https://cloud.squidex.io"
                    });

            services.AddSingleton<ISquidexClientManager>(clientManager);

            services.AddSingleton<IAssetsClient>(c =>
                c.GetRequiredService<SquidexClientManager>()
                    .CreateAssetsClient());

            services.AddSingleton<IContentsClient<ReadingFeed, ReadingFeedData>>
                (clientManager.CreateContentsClient<ReadingFeed, ReadingFeedData>("reading-feed"));
        }
    }
}
