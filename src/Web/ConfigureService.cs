﻿using Application.Contracts;
using Domain.Exceptions;
using Infrastructure.Persistence.Context;
using Infrastructure.Persistence.SeedData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;
using Web.Extensions;
using Web.Middleware;
using Web.Services;


namespace Web
{
    public static class ConfigureService
    {
        public static IServiceCollection AddWebConfigureServices(this WebApplicationBuilder builder,
            IConfiguration configuration)
        {
            // Add services to the container.
            builder.Services.AddControllers();

            // Configure Kestrel to use the specified certificate

            string certPath = Path.Combine("/app", "saelectronics.pfx");

            if (!File.Exists(certPath))
            {
                throw new FileNotFoundException("The certificate file was not found.", certPath);
            }

            builder.WebHost.ConfigureKestrel(serverOptions =>
            {
                serverOptions.ConfigureHttpsDefaults(listenOptions =>
                {
                    listenOptions.ServerCertificate = new X509Certificate2(certPath,"Sha@341401");
                });
            });

            ApiBehaviourOptions(builder);

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerDocumentation();

            builder.Services.AddCors(opt=>
            {
                opt.AddPolicy("CorsPolicy", policy =>
                {
                    policy
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .WithOrigins(
                        configuration["CorsAddress:AddressHttp"], 
                        configuration["CorsAddress:AddressHttps"])
                    .AllowCredentials();
                });
            });
            //IhttpContext Accessor
            builder.Services.AddSingleton<ICurrentUserService,CurrentUserService>();
            builder.Services.AddHttpContextAccessor();
            //cache memory
            builder.Services.AddDistributedMemoryCache();
            return builder.Services;
        }

        private static void ApiBehaviourOptions(WebApplicationBuilder builder)
        {
            builder.Services.AddExceptionHandler(options =>
            {
                options.ExceptionHandlingPath = "/error"; // مسیر مدیریت استثنائات را تنظیم کنید
            });


            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState
                    .Where(e => e.Value.Errors.Count > 0)
                    .SelectMany(v => v.Value.Errors)
                    .Select(c => c.ErrorMessage).ToList();
                    return new BadRequestObjectResult(new ApiToReturn(400, errors));
                };
            });
        }

        public static async Task<IApplicationBuilder> AddWebAppService(this WebApplication app)
        {
            app.UseHttpsRedirection();
            app.UseMiddleware<ExceptionHandlerMiddleware>();
            app.UseStaticFiles();
            //get service
            var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var loggerFactory = services.GetRequiredService<ILoggerFactory>();
            var context = services.GetRequiredService<ApplicationDbContext>();
            //auto migrations
            try
            {
                await context.Database.MigrateAsync();
                await GenerateFakeData.SeedDataAsync(context, loggerFactory);
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "error exception for migrations");
            }
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerDocumentation();
                app.UseDeveloperExceptionPage();    
            }
            else 
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseRouting();
            //CORS
            app.UseCors("CorsPolicy");
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            await app.RunAsync();
            return app;
        }
    }
}
