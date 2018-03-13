using System.Reflection;
using Abp.AspNetCore.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;

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
            //Configure navigation/menu
            Configuration.Navigation.Providers.Add<TodoNavigationProvider>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
