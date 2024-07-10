using Application;
using Infrastructure;
using Microsoft.AspNetCore.HttpOverrides;
using Web;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    //accept all networks and proxies
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});


//configurations
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.AddWebConfigureServices(builder.Configuration);
var app = builder.Build();
//ADD: use ForwardedHeaders middleware
app.UseForwardedHeaders();

//access to wwwroot
await app.AddWebAppService().ConfigureAwait(false);
