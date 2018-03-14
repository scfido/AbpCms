using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Castle.Facilities.Logging;
using Abp.AspNetCore;
using Abp.Castle.Logging.Log4Net;
using Cms.Authentication.JwtBearer;
using Cms.Configuration;
using Cms.Identity;
using Cms.Web.Resources;
using Swashbuckle.AspNetCore.Swagger;
using System.IO;

#if FEATURE_SIGNALR
using Owin;
using Abp.Owin;
using Cms.Owin;
#elif FEATURE_SIGNALR_ASPNETCORE
using Abp.AspNetCore.SignalR.Hubs;
#endif

namespace Cms.Web.Startup
{
    public class Startup
    {
        private readonly IConfigurationRoot _appConfiguration;
        private readonly IHostingEnvironment env;
        private readonly string devProjectPath;

        public Startup(IHostingEnvironment env)
        {
            _appConfiguration = env.GetAppConfiguration();
            this.env = env;
            devProjectPath = Path.GetFullPath(Path.Combine(env.ContentRootPath, "..\\Cms.Todo.Web"));
            Console.WriteLine("当前开发项目路径：" + devProjectPath);
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // MVC
            var mvcBuilder = services.AddMvc(
                options => options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute())
            );

            if (env.IsDevelopment())
            {
                //开发模式时，直接读取开发项目的View文件，产品模式则从dll的资源中读取。
                mvcBuilder.AddRazorOptions(option => option.FileProviders.Add(
                 new Microsoft.Extensions.FileProviders.PhysicalFileProvider(devProjectPath)
                ));
            }

            IdentityRegistrar.Register(services);
            AuthConfigurer.Configure(services, _appConfiguration);

            services.AddScoped<IWebResourceManager, WebResourceManager>();

#if FEATURE_SIGNALR_ASPNETCORE
            services.AddSignalR();
#endif

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info { Title = "CMS API", Version = "v1" });
                options.DocInclusionPredicate((docName, description) => true);
            });

            // Configure Abp and Dependency Injection
            return services.AddAbp<CmsWebMvcModule>(
                // Configure Log4Net logging
                options => options.IocManager.IocContainer.AddFacility<LoggingFacility>(
                    f => f.UseAbpLog4Net().WithConfig("log4net.config")
                )
            );
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseAbp(); // Initializes ABP framework.

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();
            if (env.IsDevelopment())
            {
                //开发模式时，直接读取开发项目的js、css、image等静态文件，产品模式则从dll的资源中读取。
                app.UseStaticFiles(new StaticFileOptions()
                {
                    FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(devProjectPath)
                });
            }

            //app.UseEmbeddedFiles(); //Allows to expose embedded files to the web!
            app.UseAuthentication();

            app.UseJwtTokenMiddleware();

            app.UseSwagger();
            //Enable middleware to serve swagger - ui assets(HTML, JS, CSS etc.)
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "CMS API V1");
            }); //URL: /swagger 


#if FEATURE_SIGNALR
            // Integrate with OWIN
            app.UseAppBuilder(ConfigureOwinServices);
#elif FEATURE_SIGNALR_ASPNETCORE
            app.UseSignalR(routes =>
            {
                routes.MapHub<AbpCommonHub>("/signalr");
            });
#endif

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "defaultWithArea",
                    template: "{area}/{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

#if FEATURE_SIGNALR
        private static void ConfigureOwinServices(IAppBuilder app)
        {
            app.Properties["host.AppName"] = "Cms";

            app.UseAbp();

            app.MapSignalR();
        }
#endif
    }
}
