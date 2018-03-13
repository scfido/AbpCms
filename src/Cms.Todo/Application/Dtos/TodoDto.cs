using Abp.Application.Services.Dto;

namespace Cms.Todo.Application.Dtos
{
    //[AutoMapFrom(typeof(Post))]
    public class TodoDto : EntityDto
    {
        public string Title { get; set; }

        public bool IsDone { get; set; }
    }
}