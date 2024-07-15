using Application;
using Infrastructure;
using Web;

var builder = WebApplication.CreateBuilder(args);

//configurations
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.AddWebConfigureServices(builder.Configuration);
var app = builder.Build();
//access to wwwroot
await app.AddWebAppService().ConfigureAwait(false);
