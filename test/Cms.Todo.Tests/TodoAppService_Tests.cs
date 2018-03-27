using System;
using Xunit;
using Cms.Tests;
using Cms.Todo.Application;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Shouldly;
using Cms.Todo.Application.Dtos;
using Abp.Domain.Entities;

namespace Cms.Todo.Tests
{
    public class TodoAppService_Tests : CmsTodoTestBase
    {
        private readonly TodoAppService todoAppService;

        public TodoAppService_Tests()
        {
            todoAppService = Resolve<TodoAppService>();
        }

        [Fact]
        public async Task CreateTodos_Test()
        {
            var created = await todoAppService.Create(new TodoDto() { Title = "任务1" });
            var output = await todoAppService.Get(created);
            output.Title.ShouldBe(created.Title);
        }


        [Fact]
        public async Task GetTodos_Test()
        {
            await todoAppService.Create(new TodoDto() { Title = "任务1" });
            var output = await todoAppService.GetAll(new PagedAndSortedResultRequestDto { MaxResultCount = 20, SkipCount = 0 });
            output.Items.Count.ShouldBeGreaterThan(0);
        }

        [Fact]
        public async Task Update_Test()
        {
            var created = await todoAppService.Create(new TodoDto() { Title = "任务1" });
            created.Title = "任务10";
            var output = await todoAppService.Update(created);
            output.Title.ShouldBe("任务10");
        }


        [Fact]
        public async Task DeleteTodos_Test()
        {
            var created = await todoAppService.Create(new TodoDto() { Title = "任务1" });
            await todoAppService.Delete(created);
            Should.Throw<EntityNotFoundException>(() => todoAppService.Get(created));
        }

    }
}
