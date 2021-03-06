﻿using Abp.AutoMapper;
using Abp.EntityFrameworkCore;
using Abp.EntityFrameworkCore.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Cms.EntityFrameworkCore;
using Cms.Todo.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Cms.Todo
{
    [DependsOn(typeof(CmsEntityFrameworkModule))]
    public class TodoApplicationModule : AbpModule
    {
        /* Used it tests to skip dbcontext registration, in order to use in-memory database of EF Core */
        public bool SkipDbContextRegistration { get; set; }

        public bool SkipDbSeed { get; set; }

        public override void PreInitialize()
        {
            //Adding custom AutoMapper configuration
            Configuration.Modules.AbpAutoMapper().Configurators.Add(CustomDtoMapper.CreateMappings);

            if (!SkipDbContextRegistration)
            {
                Configuration.Modules.AbpEfCore().AddDbContext<TodoDbContext>(options =>
                {
                    if (options.ExistingConnection != null)
                    {
                        CmsDbContextConfigurer<TodoDbContext>.Configure(options.DbContextOptions, options.ExistingConnection);
                    }
                    else
                    {
                        CmsDbContextConfigurer<TodoDbContext>.Configure(options.DbContextOptions, options.ConnectionString);
                    }
                });
            }

        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(TodoApplicationModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            if (!SkipDbSeed)
            {
                //SeedHelper.SeedHostDb(IocManager);
            }
        }

    }
}
