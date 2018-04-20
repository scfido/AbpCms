using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Cms.Passport.JsOAuth2Client
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = "Js OAuth2 Client";
            Console.WriteLine("这是一个使用Identity Server OAuth2授权的Html客户端");
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
