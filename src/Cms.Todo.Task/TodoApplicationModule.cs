using System;
using Abp.AspNetCore.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Cms.Todo.Application;

namespace Cms.Todo
{
    public class TodoApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Modules.AbpAspNetCore()
                .CreateControllersForAppServices(typeof(TodoApplicationModule).GetAssembly()
                );
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(TodoApplicationModule).GetAssembly());
        }

    }
}
