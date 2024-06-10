using bradjolicoeur.core.blastcms;
using bradjolicoeur.core.Helpers;
using bradjolicoeur.core.Models;
using bradjolicoeur.core.Services;
using bradjolicoeur.web;
using bradjolicoeur.web.Middleware;
using bradjolicoeur.web.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using WebEssentials.AspNetCore.Pwa;



var builder = WebApplication.CreateBuilder(args);

var Configuration = builder.Configuration;

builder.Services.Configure<ProjectOptions>(Configuration);

builder.Services.AddSingleton(typeof(IHttpContextAccessor), typeof(HttpContextAccessor));

builder.Services.AddMvc(o =>
{
    o.EnableEndpointRouting = false;
})
    .AddRazorPagesOptions(options =>
    {
        options.Conventions.AddPageRoute("/sitemap", "sitemap.xml");

    })
    .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

builder.Services.AddProgressiveWebApp(new PwaOptions
{
    AllowHttp = true
});

builder.Services.AddSingleton<IWebhookListener>(sp => new WebhookListener());

builder.Services.AddScoped<IGenerateSitemapService, GenerateSitemapService>();
builder.Services.AddTransient<ISuggestionArticlesService, SuggestionArticlesService>();

builder.Services.AddHttpClient<IBlastCMSClient, BlastCMSClient>(
    (provider, client) => {
        client.BaseAddress = new Uri(Configuration.GetValue(
            "BlastCMSBaseAddress", "https://blog-blastcms.bradjolicoeur.com/"));

    });

// Register IAppCache as a singleton CachingService
builder.Services.AddLazyCache();


var app = builder.Build();

if (Environment.Equals("Development", System.StringComparison.InvariantCultureIgnoreCase))
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

//app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCookiePolicy();

app.UseMiddleware<RedirectMiddleware>();

app.UseMvc();

app.Run();
