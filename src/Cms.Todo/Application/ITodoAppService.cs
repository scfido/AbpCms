using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Cms.Todo.Application.Dtos;

namespace Cms.Todo.Application
{
    public interface ITodoAppService : IApplicationService
    {
        Task<IList<TodoDto>>GetTodos();

    }
}
