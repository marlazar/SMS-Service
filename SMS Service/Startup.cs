using Funq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServiceStack;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using SMSService.Core.Interfaces;
using SMSService.Data.Repositories;
using SMSService.Services.Services;
using SMSService.Data.Database;

namespace SMS_Service
{
    public class Startup : ModularStartup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public new void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseServiceStack(new AppHost
            {
                AppSettings = new NetCoreAppSettings(Configuration)
            });
        }
    }

    public class AppHost : AppHostBase
    {
        public AppHost() : base("SMS_Service", typeof(SMSServices).Assembly) { }

        // Configure AppHost with the necessary configuration and dependencies
        public override void Configure(Container container)
        {
            SetConfig(new HostConfig
            {
                DefaultRedirectPath = "/metadata",
                DebugMode = AppSettings.Get(nameof(HostConfig.DebugMode), false),
            });

            container.Register<IDbConnectionFactory>(c => new OrmLiteConnectionFactory(AppSettings.GetString("Database:MySqlDbConnection"), MySqlDialect.Provider));

            container.RegisterAutoWiredAs<SMSRepository, ISMSRepository>().ReusedWithin(ReuseScope.None);
            container.RegisterAutoWiredAs<CountryRepository, ICountryRepository>().ReusedWithin(ReuseScope.None);

            using (var db = container.Resolve<IDbConnectionFactory>().Open())
            {
                Database.Create(db);
            }
        }
    }
}
