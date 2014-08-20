using Manufacturing.Api;
using Manufacturing.Api.App_Start;
using Microsoft.Owin;
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
            app.MapSignalR();

            var startupAuth = new StartupAuth();
            startupAuth.Configuration(app);
        }

        #endregion
    }
}