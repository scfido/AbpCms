using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace Cms.EntityFrameworkCore
{
    public static class CmsDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<CmsDbContext> builder, string connectionString)
        {
            builder.UseMySql(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<CmsDbContext> builder, DbConnection connection)
        {
            builder.UseMySql(connection);
        }
    }
}
