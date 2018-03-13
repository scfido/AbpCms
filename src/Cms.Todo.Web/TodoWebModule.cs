using System.Reflection;
using Abp.AspNetCore.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Resources.Embedded;

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
  
            Configuration.EmbeddedResources.Sources.Add(
                new EmbeddedResourceSet(
                    "/Areas/Todo/Views/",
                    Assembly.GetExecutingAssembly(),
                    "Cms.Todo.Web.Areas.Todo.Views"
                )
            );
            //Configure navigation/menu
            Configuration.Navigation.Providers.Add<TodoNavigationProvider>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
