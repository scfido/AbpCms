using System.Reflection;
using Abp.AspNetCore.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Resources.Embedded;
using Microsoft.AspNetCore.Hosting;

namespace Cms.Passport.Web
{
    public class CmsPassportWebModule : AbpModule
    {

        public override void PreInitialize()
        {
            var env = this.IocManager.Resolve<IHostingEnvironment>();
            if (!env.IsDevelopment())
            {
                //使用嵌入到dll中View和js、image、css等静态资源。
                //下面的代码生效的前提是在Visual Studio解决方案资源管理器中，把这些文件属性设置为嵌入的资源。
                //在开发时不希望每次改动静态资源都要重新编译，所以这段代码只在发布产品的状态下运行。
                //开发模式这些静态文件由Startup文件中单独指定了路径。
                Configuration.EmbeddedResources.Sources.Add(
                    new EmbeddedResourceSet(
                        "/Areas/Passport/Views/",
                        Assembly.GetExecutingAssembly(),
                        "Cms.Passport.Web.Areas.Passport.Views"
                    )
                );
            }
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
