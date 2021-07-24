using bradjolicoeur.core.Client;
using bradjolicoeur.core.Helpers;
using bradjolicoeur.core.Models;
using bradjolicoeur.core.Resolvers;
using bradjolicoeur.core.Services;
using bradjolicoeur.web.Middleware;
using bradjolicoeur.web.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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

            services.AddSingleton<IWebhookListener>(sp => new WebhookListener());

            services.AddScoped<IGenerateSitemapService, GenerateSitemapService>();
            services.AddTransient<ISuggestionArticlesService, SuggestionArticlesService>();

            services.AddSquidexServices(Configuration);

            // Register IAppCache as a singleton CachingService
            services.AddLazyCache();
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
