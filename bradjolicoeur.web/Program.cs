using bradjolicoeur.core.blastcms;
using bradjolicoeur.core.Helpers;
using bradjolicoeur.core.Models;
using bradjolicoeur.core.Services;
using bradjolicoeur.web.Middleware;
using bradjolicoeur.web.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using WebEssentials.AspNetCore.Pwa;

var builder = WebApplication.CreateBuilder(args);

var Configuration = builder.Configuration;

builder.Services.Configure<ProjectOptions>(Configuration);

builder.Services.AddHttpContextAccessor();

//builder.Services.AddRazorComponents();

builder.Services.AddMvc(o =>
    {
        o.EnableEndpointRouting = false;
    })
    .AddRazorPagesOptions(options =>
    {
        options.Conventions.AddPageRoute("/sitemap", "sitemap.xml");

    });

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
            "BlastCMSBaseAddress", "https://app.blastcms.net/blog-blastcms/"));

    });

// Register IAppCache as a singleton CachingService
builder.Services.AddLazyCache();

var app = builder.Build();

if (app.Environment.IsDevelopment())
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

app.UseStaticFiles();
app.UseCookiePolicy();

app.UseMiddleware<RedirectMiddleware>();

//app.MapRazorComponents<Program>();

app.UseMvc();

app.Run();
