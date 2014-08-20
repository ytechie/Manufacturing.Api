using System.Reflection;
using Bootstrap;
using log4net;
using Manufacturing.Api.App_Start;
using Microsoft.Owin;
using Microsoft.Owin.Security.ActiveDirectory;
using Owin;
using StructureMap;

namespace Manufacturing.Api.App_Start
{
    public partial class StartupAuth
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public void Configuration(IAppBuilder app)
        {
            Log.Info("Initializing authentication configuration");

            var container = (IContainer) Bootstrapper.Container;
            var config = container.GetInstance<AuthenticationConfiguration>();

            if (!config.RequireAuthentication)
            {
                Log.Warn("Authentication is disabled in the API. If this is production, this is very bad.");
                return;
            }

            Log.DebugFormat("Configuring WAAD bearer auth with audience '{0}', and tenant '{1}'", config.DirectoryDomain, config.ApiAppId);

            app.UseWindowsAzureActiveDirectoryBearerAuthentication(
                new WindowsAzureActiveDirectoryBearerAuthenticationOptions
                {
                    Audience = config.ApiAppId,
                    Tenant = config.DirectoryDomain
                });

            Log.InfoFormat("Authentication enabled using domain '{0}', and tenant '{1}'", config.DirectoryDomain, config.ApiAppId);
        }
    }
}