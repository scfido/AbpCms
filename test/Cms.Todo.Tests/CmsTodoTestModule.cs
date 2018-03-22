using Abp.Modules;
using Castle.MicroKernel.Registration;
using Cms.Tests;
using Cms.Tests.DependencyInjection;
using Cms.Todo.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cms.Todo.Tests
{
    [DependsOn(typeof(CmsTestModule), typeof(TodoApplicationModule))]
    public class CmsTodoTestModule : AbpModule
    {
        public CmsTodoTestModule(TodoApplicationModule module)
        {
            module.SkipDbContextRegistration = true;
        }

        public override void PreInitialize()
        {
        }

        public override void Initialize()
        {
            var builder = new DbContextOptionsBuilder<TodoDbContext>();
            builder.UseInMemoryDatabase(Guid.NewGuid().ToString());
            
            IocManager.IocContainer.Register(
                Component
                    .For<DbContextOptions<TodoDbContext>>()
                    .Instance(builder.Options)
                    .LifestyleSingleton()
            );
        }
    }
}
