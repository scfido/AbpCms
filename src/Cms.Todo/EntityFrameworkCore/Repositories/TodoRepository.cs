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

    public abstract class TodoRepositoryBase<TEntity, TPrimaryKey> : EfCoreRepositoryBase<TodoDbContext, TEntity, TPrimaryKey>
           where TEntity : class, IEntity<TPrimaryKey>
    {
        protected TodoRepositoryBase(IDbContextProvider<TodoDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }

    public abstract class TodoRepositoryBase<TEntity> : TodoRepositoryBase<TEntity, int>
        where TEntity : class, IEntity<int>
    {
        protected TodoRepositoryBase(IDbContextProvider<TodoDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }


    public class TodoRepository : TodoRepositoryBase<Core.Todo>, ITodoRepository
    {
        public TodoRepository(IDbContextProvider<TodoDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}