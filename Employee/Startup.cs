using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Pivotal.Discovery.Client;
using Steeltoe.Extensions.Configuration;
using Steeltoe.Extensions.Configuration.CloudFoundry;

namespace Employee
{
    public class Startup
    {
        public Startup(IHostingEnvironment env, ILoggerFactory logFactory)
        {
            logFactory.AddConsole(minLevel: LogLevel.Debug);

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables()
                // Add Steeltoe CloudFoundry provider
                .AddCloudFoundry()
                .AddConfigServer(env)
                .AddCloudFoundry()

                // Adds the Spring Cloud Configuration Server as a configuration source.
                // The settings used in contacting the Server will be picked up from
                // appsettings.json, and then overriden from any environment variables, and then
                // overriden from the CloudFoundry environment variable settings. 
                // Defaults will be used for any settings not present in any of the earlier added 
                // sources.  See ConfigServerClientSettings for defaults.
                .AddConfigServer(env, logFactory);
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();

            // Add Steeltoe CloudFoundry Options to service container
            services.Configure<CloudFoundryApplicationOptions>(Configuration);
            services.Configure<CloudFoundryServicesOptions>(Configuration);


            // Optional: Adds IConfigurationRoot as a service and also configures  IOption<ConfigServerClientSettingsOptions>,
            // IOption<CloudFoundryApplicationOptions>, IOption<CloudFoundryServicesOptions>
            // Performs:
            //      services.AddOptions();
            //      services.Configure<ConfigServerClientSettingsOptions>(config);
            //      services.Configure<CloudFoundryApplicationOptions>(config);
            //      services.Configure<CloudFoundryServicesOptions>(config);
            //      services.AddInstance<IConfigurationRoot>(config);
            services.AddConfigServer(Configuration);

            
            services.AddDiscoveryClient(Configuration);

            services.AddLogging();


            // Add framework services.
            services.AddMvc();
            services.AddDiscoveryClient(Configuration);
            services.AddConfigServer(Configuration);
            services.Configure<ConfigServerData>(Configuration);
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            
            //app.UseStaticFiles();
            app.UseMvc();
            app.UseDiscoveryClient();
        }
    }
}
