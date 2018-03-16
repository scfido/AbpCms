using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using Cms.Authorization.Roles;
using Cms.Authorization.Users;
using Cms.MultiTenancy;

namespace Cms.EntityFrameworkCore
{
    public abstract class CmsBaseDbContext<TSelf> : AbpZeroDbContext<Tenant, Role, User, TSelf> where TSelf : AbpZeroDbContext<Tenant, Role, User, TSelf>
    {
        /* Define a DbSet for each entity of the application */

        public CmsBaseDbContext(DbContextOptions<TSelf> options)
            : base(options)
        {
        }
    }

    public class CmsDbContext : CmsBaseDbContext<CmsDbContext>
    {
        /* Define a DbSet for each entity of the application */
        
        public CmsDbContext(DbContextOptions<CmsDbContext> options)
            : base(options)
        {
        }
    }
}
