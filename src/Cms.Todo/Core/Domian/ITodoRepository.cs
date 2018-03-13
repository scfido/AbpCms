using Abp.Domain.Entities;
using Abp.Domain.Repositories;

namespace Cms.Todo.Domain.Repositories
{
    public interface ITodoRepository<TEntity, TPrimaryKey> : IRepository<TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {

    }

    public interface ITodoRepository<TEntity> : IRepository<TEntity>
        where TEntity : class, IEntity<int>
    {

    }
}