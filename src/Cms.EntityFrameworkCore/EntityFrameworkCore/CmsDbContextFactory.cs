using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Cms.Configuration;
using Cms.Web;

namespace Cms.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class CmsDbContextFactory : IDesignTimeDbContextFactory<CmsDbContext>
    {
        public CmsDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<CmsDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());

            CmsDbContextConfigurer.Configure(builder, configuration.GetConnectionString(CmsConsts.ConnectionStringName));

            return new CmsDbContext(builder.Options);
        }
    }
}
