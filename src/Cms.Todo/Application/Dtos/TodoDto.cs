using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using AutoMapper;

namespace Cms.Todo.Application.Dtos
{
    [AutoMapFrom(typeof(Core.Todo), MemberList = MemberList.Source)]
    public class TodoDto : EntityDto
    {
        public string Title { get; set; }

        public bool IsDone { get; set; }
    }
}