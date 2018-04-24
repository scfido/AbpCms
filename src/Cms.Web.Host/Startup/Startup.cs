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
using Cms.Configuration;
using Cms.Identity;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using Abp.IdentityServer4;
using Cms.Authorization.Users;
using System.IO;
using Abp.PlugIns;
using Cms.Web.Core;
using Cms.Passport.Web;


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
        private readonly DeveloperProjectInfo[] devProjects;

        public Startup(IHostingEnvironment env)
        {
            _appConfiguration = env.GetAppConfiguration();
            this.env = env;
            devProjects = new DeveloperProjectInfo[]
            {
                new DeveloperProjectInfo
                {
                    AbpModule = typeof(CmsPassportWebModule),
                    Path = GetProjectPath("Cms.Passport.Web"),
                    RequstPath="/passport",
                    StaticFilePath="wwwroot"
                },
                new DeveloperProjectInfo
                {
                    AbpModule = typeof(Todo.Web.TodoWebModule),
                    Path = GetProjectPath("Cms.Todo.Web"),
                    RequstPath="/todo",
                    StaticFilePath="wwwroot"
                }
            };
        }

        private string GetProjectPath(string floder)
        {
            return Path.GetFullPath(Path.Combine(env.ContentRootPath, $"..{Path.DirectorySeparatorChar}{floder}"));
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // MVC
            var mvcBuilder = services.AddMvc(
                options => options.Filters.Add(new CorsAuthorizationFilterFactory(_defaultCorsPolicyName))
            )
            .AddDeveloperView(devProjects, env);

            IdentityRegistrar.Register(services);
            AuthConfigurer.Configure(services, _appConfiguration);

#if FEATURE_SIGNALR_ASPNETCORE
            services.AddSignalR();
#endif

            // 添加IdentityServer服务，同时也就为Host网站添加了名为"idsrv"的Cookie认证。
            // 认证默认跳转到/account/login登陆，下面指定了地址为"/passport/account/login"。
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

            // 虽然启用IdentityServer时已经添加了Cookies认证，但是任然要有调用"AddCookie()"，否则登陆成功不能跳转回源地址。
            // 客户端程序调用WebApi不会附带Cookie，这里添加了“Bearer”认证是用来给WebApi用的。
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

            //设置app_module目录下的Abp Module动态加载。
            return services.AddAbp<CmsWebHostModule>(
                // Configure Log4Net logging
                options =>
                {
                    options.IocManager.IocContainer.AddFacility<LoggingFacility>(f => f.UseAbpLog4Net().WithConfig("log4net.config"));
                    var pluginPath = GetAppModulePath();
                    if(pluginPath != null)
                        options.PlugInSources.AddFolder(pluginPath, SearchOption.TopDirectoryOnly);
                    options.PlugInSources.AddTypeList(devProjects.Select(project => project.AbpModule).ToArray());
                }
            );
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            // Initializes ABP framework.
            app.UseAbp(options =>
            {
                options.UseAbpRequestLocalization = false;
                // SecurityHeaders会在Http headers中添加“X-Frame-Options: SAMEORIGIN”
                // 导致Js客户端不能在iframe中调用connect/checksession，所有设置为false
                options.UseSecurityHeaders = false;
            });

            app.UseExceptionHandler("/error");

            app.UseCors(_defaultCorsPolicyName); // Enable CORS!

            // 这个中间件把"Bearer"认证的中Jwt附带的用户信息写入Context，
            // 使得不带Cookie的客户端程序使用JwtToken也能访问WebApi。
//            app.UseJwtTokenMiddleware("Bearer");

            //启用认证
            app.UseIdentityServer();
            app.UseStaticFiles();
            app.UseEmbeddedFiles();
            app.UseDeveloperStaticFiles(devProjects, env);

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

        private string GetAppModulePath()
        {
            var entryPath = Assembly.GetEntryAssembly().Location;
            var modulePath = $"{Path.GetDirectoryName(entryPath)}{Path.DirectorySeparatorChar}app_modules";
            if (Directory.Exists(modulePath))
                return modulePath;

            return null;
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
