using System.Threading.Tasks;
using Abp.Application.Services;
using Cms.Sessions.Dto;

namespace Cms.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
