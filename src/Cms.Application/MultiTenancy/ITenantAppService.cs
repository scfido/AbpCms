using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Cms.MultiTenancy.Dto;

namespace Cms.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}
