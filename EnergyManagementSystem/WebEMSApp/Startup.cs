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
            var provider = new TinyIoCServiceProvider();
            provider.AddTransient<HelloWorld>()
                    .AddSingleton<HelloWorld>();

            app.MapSignalR();
            app.UseDotNetify();

            //app.UseDotNetify(config =>
            //{
            //    // Register the DEMO assembly "ViewModels". All subclasses of DotNetify.BaseVM
            //    // inside that assembly will be known as view models.
            //    config.RegisterAssembly("ViewModels");

            //    // Override default factory method to provide custom objects.
            //    config.SetFactoryMethod((type, args) =>
            //    {
            //        if (type == typeof(SimpleListVM))
            //            return new SimpleListVM(new EmployeeModel(7));
            //        else if (type == typeof(BetterListVM))
            //            return new BetterListVM(new EmployeeModel(7));

            //        return ActivatorUtilities.CreateInstance(provider, type, args);
            //    });

            //}, provider);
        }
    }
}