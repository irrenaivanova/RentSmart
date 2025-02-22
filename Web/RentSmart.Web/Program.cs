﻿namespace RentSmart.Web
{
    using System;
    using System.Reflection;

    using Hangfire;
    using Hangfire.Dashboard;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using RentSmart.Common;
    using RentSmart.Data;
    using RentSmart.Data.Common;
    using RentSmart.Data.Common.Repositories;
    using RentSmart.Data.Models;
    using RentSmart.Data.Repositories;
    using RentSmart.Data.Seeding;
    using RentSmart.Services;
    using RentSmart.Services.Data;
    using RentSmart.Services.Mapping;
    using RentSmart.Services.Messaging;
    using RentSmart.Web.Infrastructure.Factories;
    using RentSmart.Web.Infrastructure.ModelBinders;
    using RentSmart.Web.ViewModels;
    using Rotativa.AspNetCore;

    using static RentSmart.Web.Infrastructure.Extensions.WebApplicationBuilderExtensions;

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // For Railway
            // builder.WebHost.UseUrls("http://*:8080");

            // overriding appsettings.development with appsettings.production
            builder.Configuration
                .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
                .AddJsonFile("appsettings.Production.json", optional: false, reloadOnChange: true);


            // var connectionString = builder.Configuration.GetConnectionString("ApplicationDbContextConnection") ?? throw new InvalidOperationException("Connection string 'ApplicationDbContextConnection' not found.");
            ConfigureServices(builder.Services, builder.Configuration);

            var app = builder.Build();
            Configure(app);
            app.Run();
        }

        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<PropertySeeder>();
            services.AddScoped<ISeeder>(provider => provider.GetRequiredService<PropertySeeder>());

            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<ApplicationUser>(IdentityOptionsProvider.GetIdentityOptions)
                .AddRoles<ApplicationRole>().AddEntityFrameworkStores<ApplicationDbContext>();

            // Add Hangfire services, SQL storage and server
            var hangfireConnectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddHangfire(configuration => configuration.UseSqlServerStorage(hangfireConnectionString));
            services.AddHangfireServer();

            // Register the custom claims principal factory
            services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, CustomUserClaimsPrincipalFactory>();
            services.Configure<CookiePolicyOptions>(
                options =>
                {
                    options.CheckConsentNeeded = context => true;
                    options.MinimumSameSitePolicy = SameSiteMode.None;
                });

            services.AddAuthentication().AddFacebook(opt =>
            {
                opt.AppId = configuration["Authentication:Facebook:AppId"];
                opt.AppSecret = configuration["Authentication:Facebook:AppSecret"];
            })
            .AddGoogle(opt =>
            {
                opt.ClientId = configuration["Authentication:Google:ClientId"];
                opt.ClientSecret = configuration["Authentication:Google:ClientSecret"];
            });

            services.AddControllersWithViews(
                options =>
                {
                    options.ModelBinderProviders.Insert(0, new DecimalModelBinderProvider());
                    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                }).AddRazorRuntimeCompilation();
            services.AddRazorPages();
            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddAntiforgery(options =>
            {
                options.HeaderName = "X-CSRF-TOKEN";
            });

            services.AddSingleton(configuration);

            // Data repositories
            services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped<IDbQueryRunner, DbQueryRunner>();

            // Application services
            services.AddTransient<IEmailSender>(x => new SendGridEmailSender(configuration["SendGrid:ApiKey"]));

            // Add all services from assembly of PropertyService through extension method using reflection
            services.AddApplicationServices(typeof(PropertyService));

            services.AddScoped<RentalNotificationService>();

        }

        private static void Configure(WebApplication app)
        {
            // Seed data on application startup
            using (var serviceScope = app.Services.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.Migrate();
                new ApplicationDbContextSeeder().SeedAsync(dbContext, serviceScope.ServiceProvider).GetAwaiter().GetResult();
            }

            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error/500");
                app.UseStatusCodePagesWithRedirects("/Home/Error?statusCode={0}");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            // Enable Hangfire Dashboard
            app.UseHangfireDashboard("/hangfire");

            if (app.Environment.IsProduction())
            {
                app.UseHangfireDashboard(
                    "/hangfire",
                    new DashboardOptions { Authorization = new[] { new HangfireAuthorizationFilter() } });
            }

            // Add recurring job for notifyExpiringRentals
            RecurringJob.AddOrUpdate<RentalNotificationService>("NotifyExpiringRentals", service => service.NotifyExpiringRentals(), Cron.Daily);

            // adding cache control headers trying to resolve the problem with showing cookie consent
            app.Use(async (context, next) =>
            {
                context.Response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate";
                context.Response.Headers["Pragma"] = "no-cache";
                context.Response.Headers["Expires"] = "0";
                await next();
            });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            // Rotativa
            RotativaConfiguration.Setup(app.Environment.WebRootPath, "Rotativa");

            app.MapControllerRoute("areaRoute", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
            app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();
        }

        private class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
        {
            public bool Authorize(DashboardContext context)
            {
                var httpContext = context.GetHttpContext();
                if (httpContext.User.Identity.IsAuthenticated)
                {
                    return httpContext.User.IsInRole(GlobalConstants.AdministratorRoleName);
                }

                return false;
            }
        }
    }
}
