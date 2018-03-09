using System.Threading.Tasks;
using Abp.Application.Services;
using Cms.Authorization.Accounts.Dto;

namespace Cms.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

        Task<RegisterOutput> Register(RegisterInput input);
    }
}
