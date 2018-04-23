using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

namespace Cms.Web.Core
{
    /// <summary>
    /// </summary>
    public static class DeveloperExtensions
    {
        /// <summary>
        /// 开发模式时，直接读取开发项目的js、css、image等静态文件，产品模式则读取嵌入dll的资源。
        /// </summary>
        /// <param name="app"></param>
        /// <param name="projects"></param>
        /// <param name="env"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseDeveloperStaticFiles(
            this IApplicationBuilder app,
            IEnumerable<DeveloperProjectInfo> projects, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                foreach (var project in projects)
                {
                    if (project.StaticFilePath == null)
                        continue;

                    app.UseStaticFiles(new StaticFileOptions()
                    {
                        FileProvider = new PhysicalFileProvider(Path.Combine(project.Path, project.StaticFilePath)),
                        RequestPath = project.RequstPath
                    });
                }
            }

            return app;
        }


        /// <summary>
        /// 开发模式时，直接读取开发项目的View文件，产品模式则从dll的资源中读取。
        /// </summary>
        /// <param name="services"></param>
        /// <param name="projects"></param>
        /// <param name="env"></param>
        /// <returns></returns>
        public static IMvcBuilder AddDeveloperView(
            this IMvcBuilder mvcBuilder,
            IEnumerable<DeveloperProjectInfo> projects, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                mvcBuilder.AddRazorOptions(options =>
                {
                    foreach (var project in projects)
                    {
                        options.FileProviders.Add(new PhysicalFileProvider(project.Path));
                    }
                });
            }

            return mvcBuilder;
        }
    }
}