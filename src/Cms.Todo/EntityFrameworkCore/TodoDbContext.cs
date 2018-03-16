using Abp.Domain.Repositories;
using Abp.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using Cms.Authorization.Roles;
using Cms.Authorization.Users;
using Cms.EntityFrameworkCore;
using Cms.MultiTenancy;
using Cms.Todo.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cms.Todo.EntityFrameworkCore
{
    public class TodoDbContext : CmsBaseDbContext<TodoDbContext>
    {
        public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options)
        {
        }

        public DbSet<Core.Todo> Todos { get; set; }
    }

}
