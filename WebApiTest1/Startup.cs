using Microsoft.Owin;
using System.Web.Http;
using WebApiTest1;
using System;
using Owin;
using Microsoft.Owin.Cors;
using Newtonsoft.Json.Serialization;


[assembly: OwinStartup(typeof(Startup))]
namespace WebApiTest1
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();

            WebApiConfig.Register(config);

           

            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

            app.UseCors(CorsOptions.AllowAll);
            

        }
    }

}