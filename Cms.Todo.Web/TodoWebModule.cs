using System.Reflection;
using Abp.Modules;

namespace Cms.Todo.Web
{
    [DependsOn(
         typeof(TodoApplicationModule)
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
