using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Castle.Facilities.Logging;
using Swashbuckle.AspNetCore.Swagger;
using Abp.AspNetCore;
using Abp.Castle.Logging.Log4Net;
using Abp.Extensions;
using Cms.Authentication.JwtBearer;
using Cms.Configuration;
using Cms.Identity;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using Abp.IdentityServer4;
using Cms.Authorization.Users;
using Cms.Web.Host.IdentityServer;
using System.IO;
using Microsoft.AspNetCore.Authentication;


#if FEATURE_SIGNALR
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Owin;
using Abp.Owin;
using Cms.Owin;
#elif FEATURE_SIGNALR_ASPNETCORE
using Abp.AspNetCore.SignalR.Hubs;
#endif

namespace Cms.Web.Host.Startup
{
    public class Startup
    {
        private const string _defaultCorsPolicyName = "localhost";

        private readonly IConfigurationRoot _appConfiguration;
        private readonly IHostingEnvironment env;
        private readonly string devProjectPath;

        public Startup(IHostingEnvironment env)
        {
            _appConfiguration = env.GetAppConfiguration();
            this.env = env;
            var devProject = "Cms.Passport.Web";
            devProjectPath = Path.GetFullPath(Path.Combine(env.ContentRootPath, $"..{Path.DirectorySeparatorChar}{devProject}"));
            Console.WriteLine("当前开发项目路径：" + devProjectPath);
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // MVC
            var mvcBuilder = services.AddMvc(
                options => options.Filters.Add(new CorsAuthorizationFilterFactory(_defaultCorsPolicyName))
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

#if FEATURE_SIGNALR_ASPNETCORE
            services.AddSignalR();
#endif

            // 这段代码作用是添加IdentityServer服务，和Cms网站的认证是无关的。但是
            // AddIdentityServer()会自动添加名为"idsrv"的认证，用于IdentityServer服务
            // 自身的认证，他的方式Cookies认证类似，默认跳转到/account/login登陆，下
            // 面指定了地址为"/passport/account/login"。
            services.AddIdentityServer(options =>
            {
                options.UserInteraction.LoginUrl = "/passport/account/login";
                options.UserInteraction.LogoutUrl = "/passport/account/logout";
                options.UserInteraction.ErrorUrl = "/passport/home/error";
                options.UserInteraction.ConsentUrl = "/passport/consent";
            })
            .AddSigningCredential(
                new X509Certificate2(_appConfiguration.GetValue<string>("IdentityServer:Certificate:File"),
                _appConfiguration.GetValue<string>("IdentityServer:Certificate:Password"))
                )
            .AddInMemoryIdentityResources(IdentityServerConfig.GetIdentityResources())
            .AddInMemoryApiResources(IdentityServerConfig.GetApiResources())
            .AddInMemoryClients(IdentityServerConfig.GetClients())
            .AddAbpPersistedGrants<IAbpPersistedGrantDbContext>()
            .AddAbpIdentityServer<User>()
            .AddRedirectUriValidator<AnyRedirectUriValidator>();

            // 这段代码是启用Host这个网站的Bearer认证，因Host与IdentityServer合并
            // 在同一网站，所以Host的Mvc页面已经具备Cookie认证，就不需要添再添加
            // Cookie认证了。IdentityServer要从Host网站剥离出，就需要添加Cookie认
            // 证来保护Mvc页面。客户端程序调用WebApi不会附带Cookie，这里添加了“Bearer”
            // 就是用来认证WebApi的。
            services.AddAuthentication()
                .AddCookie()
                .AddIdentityServerAuthentication("Bearer", options =>
                 {
                     //TODO:应该改为从配置文件获取，如果未设置就以本服务作为认证服务。
                     options.Authority = "http://localhost:5000";    //认证服务的地址
                     options.RequireHttpsMetadata = false;
                     options.ApiName = "default-api";
                 });

            // Configure CORS for React UI
            services.AddCors(
                options => options.AddPolicy(
                    _defaultCorsPolicyName,
                    builder => builder
                        .WithOrigins(
                            // App:CorsOrigins in appsettings.json can contain more than one address separated by comma.
                            _appConfiguration["App:CorsOrigins"]
                                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                                .Select(o => o.RemovePostFix("/"))
                                .ToArray()
                        )
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                )
            );

            if (env.IsDevelopment())
            {
                // Swagger - Enable this line and the related lines in Configure method to enable swagger UI
                services.AddSwaggerGen(options =>
                {
                    options.SwaggerDoc("v1", new Info { Title = "Cms API", Version = "v1" });
                    options.DocInclusionPredicate((docName, description) => true);

                    // Define the BearerAuth scheme that's in use
                    options.AddSecurityDefinition("bearerAuth", new ApiKeyScheme()
                    {
                        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                        Name = "Authorization",
                        In = "header",
                        Type = "apiKey"
                    });
                    // Assign scope requirements to operations based on AuthorizeAttribute
                    options.OperationFilter<SecurityRequirementsOperationFilter>();
                });
            }

            // Configure Abp and Dependency Injection
            return services.AddAbp<CmsWebHostModule>(
                // Configure Log4Net logging
                options => options.IocManager.IocContainer.AddFacility<LoggingFacility>(
                    f => f.UseAbpLog4Net().WithConfig("log4net.config")
                )
            );
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            // Initializes ABP framework.
            app.UseAbp(options => {
                options.UseAbpRequestLocalization = false;
                // SecurityHeaders会在Http headers中添加“X-Frame-Options: SAMEORIGIN”
                // 导致Js客户端不能在iframe中调用connect/checksession，所有设置为false
                options.UseSecurityHeaders = false;     
            }); 

            app.UseExceptionHandler("/error");

            app.UseCors(_defaultCorsPolicyName); // Enable CORS!
                                                 //app.Use(async (ctx, next) => {

            //    var result = await ctx.AuthenticateAsync();
            //    var userManager = Abp.Dependency.IocManager.Instance.Resolve<UserManager>();
            //    var user = await userManager.GetUserAsync(result.Principal);
            //});


            // 这个中间件把"Bearer"认证的中Jwt附带的用户信息写入Context，
            // 使得不带Cookie的客户端程序使用JwtToken也能访问WebApi。
            app.UseJwtTokenMiddleware("Bearer");

            //启用认证
            app.UseIdentityServer();
            app.UseStaticFiles();
            if (env.IsDevelopment())
            {
                //开发模式时，直接读取开发项目的js、css、image等静态文件，产品模式则从dll的资源中读取。
                app.UseStaticFiles(new StaticFileOptions()
                {
                    FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider($"{ devProjectPath }{ Path.DirectorySeparatorChar }wwwroot"),
                    RequestPath = "/passport"
                });
            }

            app.UseAbpRequestLocalization();

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

            if (env.IsDevelopment())
            {
                app.UseSwagger();
                //Enable middleware to serve swagger - ui assets(HTML, JS, CSS etc.)
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "CMS API V1");
                    options.DocumentTitle = "CMS API";
                }); //URL: /swagger 
            }
        }

#if FEATURE_SIGNALR
        private static void ConfigureOwinServices(IAppBuilder app)
        {
            app.Properties["host.AppName"] = "Cms";

            app.UseAbp();
            
            app.Map("/signalr", map =>
            {
                map.UseCors(CorsOptions.AllowAll);
                var hubConfiguration = new HubConfiguration
                {
                    EnableJSONP = true
                };
                map.RunSignalR(hubConfiguration);
            });
        }
#endif
    }
}
