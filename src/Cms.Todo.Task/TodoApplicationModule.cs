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
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(TodoApplicationModule).GetAssembly());
            //Configuration.Modules.AbpWebApi().DynamicApiControllerBuilder.For<ITodoAppServices>("todo").Build();
        }

    }
}
