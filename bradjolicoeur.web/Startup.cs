using bradjolicoeur.core.Client;
using bradjolicoeur.core.Helpers;
using bradjolicoeur.core.Models;
using bradjolicoeur.core.Resolvers;
using bradjolicoeur.core.Services;
using bradjolicoeur.Core.Models.ContentType;
using bradjolicoeur.web.Filters;
using bradjolicoeur.web.Middleware;
using bradjolicoeur.web.Resolvers;
using KenticoCloud.Delivery;
using Markdig;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using WebEssentials.AspNetCore.Pwa;

namespace bradjolicoeur.web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<ProjectOptions>(Configuration);

            //services.Configure<CookiePolicyOptions>(options =>
            //{
            //    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
            //    options.CheckConsentNeeded = context => true;
            //    options.MinimumSameSitePolicy = SameSiteMode.None;
            //});

            services.AddSingleton(typeof(IHttpContextAccessor), typeof(HttpContextAccessor));

            services.AddMvc(o =>
                {
                    o.EnableEndpointRouting = false;
                })
                .AddRazorPagesOptions(options =>
                {
                    options.Conventions.AddPageRoute("/sitemap", "sitemap.xml");
                   
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.AddProgressiveWebApp( new PwaOptions
            {
                AllowHttp = true
            });

            var deliveryOptions = new DeliveryOptions();
            Configuration.GetSection(nameof(DeliveryOptions)).Bind(deliveryOptions);

            services.AddSingleton<IWebhookListener>(sp => new WebhookListener());
            services.AddSingleton<IDependentTypesResolver>(sp => new DependentFormatResolver());
            services.AddSingleton<ICacheManager>(sp => new ReactiveCacheManager(
                sp.GetRequiredService<IOptions<ProjectOptions>>(),
                sp.GetRequiredService<IMemoryCache>(),
                sp.GetRequiredService<IDependentTypesResolver>(),
                sp.GetRequiredService<IWebhookListener>()));
            services.AddScoped<KenticoCloudSignatureActionFilter>();

            services.AddSingleton<IDeliveryClient>(sp => new CachedDeliveryClient(
                sp.GetRequiredService<IOptions<ProjectOptions>>(),
                sp.GetRequiredService<ICacheManager>(),
                DeliveryClientBuilder.WithOptions(_ => deliveryOptions)
                .WithCodeFirstTypeProvider(new CustomTypeProvider())
                .WithContentLinkUrlResolver(new CustomContentLinkUrlResolver())
                .Build())
               );

            services.AddScoped<IGenerateSitemapService, GenerateSitemapService>();


            services.AddSquidexServices();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.EnvironmentName.Equals("Development", System.StringComparison.InvariantCultureIgnoreCase))
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {

                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMiddleware<RedirectMiddleware>();

            app.UseMvc();
        }
    }
}
