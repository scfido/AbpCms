using Abp.Modules;
using Abp.Reflection.Extensions;

namespace Cms.Todo
{
    public class TodoApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(TodoApplicationModule).GetAssembly());
        }

    }
}
