using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;
using Arda.Common.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Arda.Main
{
    public partial class Startup
    {
        public IConfigurationRoot Configuration { get; set; }

        public Startup(IHostingEnvironment env)
        {
            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json")
                .AddJsonFile("local-secret.json", true)
                .AddJsonFile("microservices.json", true)
                .AddEnvironmentVariables();

            if (env.IsDevelopment())
            {
                builder.AddUserSecrets("arda-20160816073715");
            }

            if (!env.IsProduction())
            {
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            Configuration = builder.Build();

            //Injecting endpoints
            Arda.Common.Utils.Util.SetEnvironmentVariables(Configuration);

            TestManager.TestStatic();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new RequireHttpsAttribute());
            });

            services.AddCors(x => x.AddPolicy("AllowAll", c => c.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

            // Add framework services.
            services.AddApplicationInsightsTelemetry(Configuration);
            
            //Add Session Middleware
            services.AddMemoryCache();
            services.AddSession();

            services.AddMvc()
                .AddJsonOptions(opts =>
                {
                    opts.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();
                });

            services.AddAuthentication(sharedOptions => sharedOptions.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme);

            // Registering distributed cache approach to the application.
            services.AddSingleton<IDistributedCache>(serviceProvider => new RedisCache(new RedisCacheOptions
            {
                Configuration = Configuration.Get("Storage_Redis_Configuration"),
                InstanceName = Configuration.Get("Storage_Redis_InstanceName")
            }));

            services.AddSingleton<TestManager, TestManager>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, TestManager testManager)
        {
            loggerFactory.AddConsole(LogLevel.Trace);
            var logger = loggerFactory.CreateLogger("Default");

            var options = new RewriteOptions().AddRedirectToHttps();

            app.UseRewriter(options);

            app.UseApplicationInsightsRequestTelemetry();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseApplicationInsightsExceptionTelemetry();

            app.UseStaticFiles();
            app.UseDeveloperExceptionPage();
            app.UseSession();

            // Configure the OpenIdConnect Auth Pipeline and required services.
            ConfigureAuth(app);
            //// Configure Security on Main:
            //app.UseMiddleware<SecurityMainMiddleware>();

            app.UseCors("AllowAll");

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            UsageTelemetry.Track("[system]", ArdaUsage.ArdaMain_Start);

            if(env.IsDevelopment())
            {
                testManager.TestMain();
            }
        }
    }
}