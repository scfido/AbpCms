using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace Cms.EntityFrameworkCore
{
    public static class CmsDbContextConfigurer<T> where T: DbContext
    {
        public static void Configure(DbContextOptionsBuilder<T> builder, string connectionString)
        {
            builder.UseMySql(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<T> builder, DbConnection connection)
        {
            builder.UseMySql(connection);
        }
    }
}
