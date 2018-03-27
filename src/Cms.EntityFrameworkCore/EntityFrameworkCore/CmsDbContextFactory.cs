using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Cms.Configuration;
using Cms.Web;
using System;

namespace Cms.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class CmsDbContextFactory : CmsDbContextFactory<CmsDbContext> 
    {
    }

    public abstract class CmsDbContextFactory<T> : IDesignTimeDbContextFactory<T> where T : DbContext
    {
        public T CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<T>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());

            CmsDbContextConfigurer<T>.Configure(builder, configuration.GetConnectionString(CmsConsts.ConnectionStringName));
            var dbContext = (T)Activator.CreateInstance(typeof(T), builder.Options);
            return dbContext;
        }
    }

}
