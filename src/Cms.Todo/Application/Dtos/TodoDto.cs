using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using AutoMapper;

namespace Cms.Todo.Application.Dtos
{
    public class TodoDto : EntityDto
    {
        public string Title { get; set; }

        public bool IsDone { get; set; }
    }
}