using System;
using Xunit;
using Cms.Tests;
using Cms.Todo.Application;

namespace Cms.Todo.Tests
{
    public class TodoAppService_Tests : CmsTestBase
    {
        private readonly ITodoAppService todoAppService;

        public TodoAppService_Tests()
        {
            todoAppService = Resolve<ITodoAppService>();
        }

        [Fact]
        public async Task GetTodos_Test()
        {
            // Act
            var output = await todoAppService.GetAll(new PagedResultRequestDto{MaxResultCount=20, SkipCount=0} );

            // Assert
            output.Items.Count.ShouldBeGreaterThan(0);
        }   
    }
}
