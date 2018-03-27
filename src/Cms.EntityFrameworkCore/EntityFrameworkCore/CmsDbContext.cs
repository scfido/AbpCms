using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using Cms.Authorization.Roles;
using Cms.Authorization.Users;
using Cms.MultiTenancy;

namespace Cms.EntityFrameworkCore
{
    public class CmsDbContext : AbpZeroDbContext<Tenant, Role, User, CmsDbContext>
    {
        /* Define a DbSet for each entity of the application */
        
        public CmsDbContext(DbContextOptions<CmsDbContext> options)
            : base(options)
        {
        }
    }
}
