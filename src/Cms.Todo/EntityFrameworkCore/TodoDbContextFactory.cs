using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Cms.Configuration;
using Cms.Web;
using Cms.EntityFrameworkCore;

namespace Cms.Todo.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class TodoDbContextFactory : CmsDbContextFactory<TodoDbContext>
    {
    }
}
