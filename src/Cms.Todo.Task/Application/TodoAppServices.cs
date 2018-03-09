﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Cms.Todo.Application.Dtos;

namespace Cms.Todo.Application
{
    public class TodoAppServices : ApplicationService, ITodoAppServices
    {
        public TodoAppServices()
        {
        }

        public async Task<IList<TodoDto>> GetTodos(GetTodosInput input)
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
