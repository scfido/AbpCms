using System;
using Microsoft.IdentityModel.Tokens;

namespace Cms.Web.Core
{
    /// <summary>
    /// 开发中的项目信息
    /// </summary>
    public class DeveloperProjectInfo
    {
        /// <summary>
        /// 该项目http请求路径前缀，必须是以/开头的绝对路径。例如：“/passport，/app/todo”
        /// </summary>
        public string RequstPath { get; set; }

        /// <summary>
        /// 项目的所在路径，末尾不要带路径分隔符。
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 项目的静态文件文件夹。例如“wwwroot”
        /// </summary>
        public string StaticFilePath { get; set; }
    }
}
