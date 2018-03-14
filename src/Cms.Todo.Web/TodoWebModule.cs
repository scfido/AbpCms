using System.Reflection;
using Abp.AspNetCore.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Resources.Embedded;
using Microsoft.AspNetCore.Hosting;

namespace Cms.Todo.Web
{
    [DependsOn(
         typeof(TodoApplicationModule)
         )]
    public class TodoWebModule : AbpModule
    {

        public override void PreInitialize()
        {

            Configuration.Modules.AbpAspNetCore()
                .CreateControllersForAppServices(typeof(TodoApplicationModule).GetAssembly()
            );

            var env = this.IocManager.Resolve<IHostingEnvironment>();
            if (!env.IsDevelopment())
            {
                //使用源嵌入到dll中View和js、image、css等静态资。
                //下面的代码生效的前提是在Visual Studio解决方案资源管理器中，把这些静态资源
                //设置为嵌入的资源。
                //在开发时不希望每次改动静态资源都要重新编译，所以这段代码只在发布产品的状态下运行。
                //开发模式这些静态文件由Startup文件中单独指定了路径。
                Configuration.EmbeddedResources.Sources.Add(
                    new EmbeddedResourceSet(
                        "/Areas/Todo/Views/",
                        Assembly.GetExecutingAssembly(),
                        "Cms.Todo.Web.Areas.Todo.Views"
                    )
                );
            }

            //Configure navigation/menu
            Configuration.Navigation.Providers.Add<TodoNavigationProvider>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
