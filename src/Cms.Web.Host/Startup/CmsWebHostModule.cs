using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Cms.Configuration;
using Cms.Todo.Web;

namespace Cms.Web.Host.Startup
{
    [DependsOn(
       typeof(CmsWebCoreModule),
        typeof(TodoWebModule))]
    public class CmsWebHostModule: AbpModule
    {
        private readonly IHostingEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public CmsWebHostModule(IHostingEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(CmsWebHostModule).GetAssembly());
        }
    }
}
