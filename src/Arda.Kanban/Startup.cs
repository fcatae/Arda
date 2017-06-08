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
using Arda.Common.Utils;

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
                Configuration = Configuration.Get("Storage_Redis_Configuration"),
                InstanceName = Configuration.Get("Storage_Redis_InstanceName")
            }));

            //// Adding database connection by dependency injection.
            var connectionString = Configuration.Get("Storage_SqlServer_Kanban_ConnectionString");
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
            services.AddScoped<IWorkspaceRepository, WorkspaceRepository>();

            services.AddSingleton<TestManager, TestManager>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Arda.Kanban", Version = "v1" });
                c.SwaggerDoc("v2", new Info { Title = "Arda.Kanban v2 (Workspaces)", Version = "v2" });

                c.DocInclusionPredicate((docName, apiDesc) =>
                {
                    switch (docName)
                    {
                        case "v1":
                            return apiDesc.RelativePath.StartsWith("api/");
                        case "v2":
                            return apiDesc.RelativePath.StartsWith("v2/");
                    }

                    // unknown version?
                    return true;
                });

                c.OperationFilter<MultipleOperationsWithSameVerbFilter>();
            });            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, TestManager testmgr)
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
                c.SwaggerEndpoint("/swagger/v2/swagger.json", "Arda.Kanban v2 (Workspaces)");
            });
            
            app.UseMvc();

            if(env.IsDevelopment())
            {
                testmgr.Run();
            }
        }

        // Entry point for the application.
        //public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
