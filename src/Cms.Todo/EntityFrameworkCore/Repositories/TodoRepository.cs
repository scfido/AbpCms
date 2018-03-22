using Abp.Domain.Entities;
using Abp.EntityFrameworkCore;
using Abp.EntityFrameworkCore.Repositories;
using Cms.EntityFrameworkCore;
using Cms.EntityFrameworkCore.Repositories;
using Cms.Todo.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cms.Todo.EntityFrameworkCore.Repositories
{
    public class TodoRepository : EfCoreRepositoryBase<TodoDbContext, Core.Todo>, ITodoRepository
    {
        public TodoRepository(IDbContextProvider<TodoDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}