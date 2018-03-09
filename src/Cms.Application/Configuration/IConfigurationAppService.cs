using System.Threading.Tasks;
using Cms.Configuration.Dto;

namespace Cms.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
