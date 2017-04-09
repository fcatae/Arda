using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;

namespace Arda.Main
{
    public partial class Startup
    {
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

        }

        public IConfigurationRoot Configuration { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            //Injecting endpoints
            Arda.Common.Utils.Util.SetEnvironmentVariables(Configuration);

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

            // Registering distributed cache approach to the application.
            services.AddSingleton<IDistributedCache>(serviceProvider => new RedisCache(new RedisCacheOptions
            {
                Configuration = Configuration["Storage:Redis:Configuration"],
                InstanceName = Configuration["Storage:Redis:InstanceName"]
            }));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(LogLevel.Trace);
            var logger = loggerFactory.CreateLogger("Default");
            
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

            UsageTelemetry.Track(null, ArdaUsage.ArdaMain_Start);
        }
    }
}