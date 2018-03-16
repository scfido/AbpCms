using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Domain.Repositories;
using Cms.Todo.Application.Dtos;
using Cms.Todo.Core;
using Cms.Todo.Domain.Repositories;

namespace Cms.Todo.Application
{
    public class TodoAppService : AsyncCrudAppService<Core.Todo, TodoDto>, ITodoAppService
    {
        public TodoAppService(ITodoRepository repository) : base(repository)
        {
            
        }

        

        public async Task<IList<TodoDto>> GetTodos()
        {
            return new List<TodoDto>
            { 
                new TodoDto() { Id=1, Title = "任务1" }, 
                new TodoDto() { Id=2, Title = "任务2" }, 
                new TodoDto() { Id=3, Title = "任务3" , IsDone=true} 
            };
        }
    }
}
