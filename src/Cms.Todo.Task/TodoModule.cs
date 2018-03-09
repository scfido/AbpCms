using System;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Cms.Todo.Web;
using Abp.Application.Services;
using Abp.WebApi;
using Abp.WebApi.Controllers.Dynamic.Builders;

namespace Cms.Todo
{
    public class TodoModule : AbpModule
    {
        public override void PreInitialize()
        {

        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(TodoModule).GetAssembly());
        }

    }
}
