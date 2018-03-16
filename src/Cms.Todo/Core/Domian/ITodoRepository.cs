using Abp.Domain.Entities;
using Abp.Domain.Repositories;

namespace Cms.Todo.Domain.Repositories
{
    public interface ITodoRepository : IRepository<Core.Todo>
    {

    }
}