using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;
using Arda.Kanban.Models;
using Arda.Kanban.Models.Repositories;
using Arda.Kanban.Repositories;
using Swashbuckle.AspNetCore.Swagger;

namespace Arda.Kanban
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json")
                .AddJsonFile("local-secret.json", true)
                .AddJsonFile("microservices.json", true);

                if (env.IsDevelopment())
                {
                    builder.AddUserSecrets();
                }
                if (!env.IsProduction())
                {
                    // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                    builder.AddApplicationInsightsSettings(developerMode: true);
                }

            builder = builder.AddEnvironmentVariables();
            Configuration = builder.Build();//.ReloadOnChanged("appsettings.json");
        }

        public IConfigurationRoot Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            Arda.Common.Utils.Util.SetEnvironmentVariables(Configuration);

            // Add framework services.
            services.AddCors(x => x.AddPolicy("AllowAll", c => c.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

            services.AddApplicationInsightsTelemetry(Configuration);

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

            //// Adding database connection by dependency injection.
            var connectionString = Configuration["Storage:SqlServer-Kanban:ConnectionString"];
            services.AddDbContext<KanbanContext>(options => options.UseSqlServer(connectionString));

            //Registering services.
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IFiscalYearRepository, FiscalYearRepository>();
            services.AddScoped<IMetricRepository, MetricRepository>();
            services.AddScoped<IActivityRepository, ActivityRepository>();
            services.AddScoped<ITechnologyRepository, TechnologyRepository>();
            services.AddScoped<IWorkloadRepository, WorkloadRepository>();
            services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            services.AddScoped<IReportRepository, ReportRepository>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Arda.Kanban", Version = "v1" });
            });            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            //app.UseMiddleware<SecurityAPIMiddleware>();

            app.UseApplicationInsightsRequestTelemetry();

            app.UseApplicationInsightsExceptionTelemetry();

            app.UseStaticFiles();

            app.UseCors("AllowAll");

            app.UseSwagger();
            app.UseSwaggerUi(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Arda.Kanban v1");
            });
            
            app.UseMvc();
        }

        // Entry point for the application.
        //public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
