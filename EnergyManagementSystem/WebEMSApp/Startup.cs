using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using DotNetify;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(WebEMSApp.Startup))]
namespace WebEMSApp
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        //public void ConfigureServices(IServiceCollection services)
        //{
        //    services.AddMemoryCache();
        //    services.AddSignalR();
        //    services.AddDotNetify();
        //}

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IAppBuilder app)
        {
            app.MapSignalR();
            app.UseDotNetify();
        }
    }
}
