using System.Reflection;
using Abp.Modules;
using Abp.Web.Mvc;

namespace Cms.Todo.Web
{
    [DependsOn(
         typeof(TodoModule),
         typeof(AbpWebMvcModule)
         )]
    public class TodoWebModule : AbpModule
    {

        public override void PreInitialize()
        {

            //Configure navigation/menu
            Configuration.Navigation.Providers.Add<TodoNavigationProvider>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
