using bradjolicoeur.web;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;



var builder = WebApplication.CreateBuilder(args);

// Manually create an instance of the Startup class
var startup = new Startup(builder.Configuration);

// Manually call ConfigureServices()
startup.ConfigureServices(builder.Services);

var app = builder.Build();

// Fetch all the dependencies from the DI container 
// var hostLifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();
// As pointed out by DavidFowler, IHostApplicationLifetime is exposed directly on ApplicationBuilder

// Call Configure(), passing in the dependencies
startup.Configure(app, app.Lifetime);

app.Run();
