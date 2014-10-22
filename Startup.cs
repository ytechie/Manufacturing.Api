using Manufacturing.Api;
using Manufacturing.Api.App_Start;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;

//using Microsoft.AspNet.SignalR.ServiceBus;
//using Microsoft.AspNet.SignalR;

[assembly: OwinStartup(typeof(Startup))]

namespace Manufacturing.Api
{
    public class Startup
    {
        #region public

        public void Configuration(IAppBuilder app)
        {
            //uncomment the two lines below the two using statements for using ServiceBus backplane
            /*
            string sbcon = @"Endpoint=sb://signal-scaleout.servicebus.windows.net/;SharedSecretIssuer=owner;SharedSecretValue=0C48TVfiEaQq3yMklA0xIrejzgTPxsrcQwyNI7Cw+wQ=";
            GlobalHost.DependencyResolver.UseServiceBus(sbcon, "plantmonitor");
             */

            app.Map("/signalr", map =>
            {
                map.UseCors(CorsOptions.AllowAll);

                var hubConfiguration = new HubConfiguration();

                map.RunSignalR(hubConfiguration);
            });

            app.Map("/api", map =>
            {
                map.UseCors(CorsOptions.AllowAll);
            });

            //app.MapSignalR("/signalr", new HubConfiguration());

            var startupAuth = new StartupAuth();
            startupAuth.Configuration(app);
        }

        #endregion
    }
}