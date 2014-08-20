using System;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.WebHost;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Bootstrap;
using Bootstrap.Locator;
using Bootstrap.StructureMap;
using log4net;
using Manufacturing.Framework.Configuration;
using Manufacturing.Framework.Logging;
using StructureMap;
using Manufacturing.Api.DependencyResolution;

namespace Manufacturing.Api
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static IContainer _container;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            LoggingUtils.InitializeLogging(true);
            Log.Info("Manufacturing API Starting");

            Log.Debug("Loading Configuration...");
            Bootstrapper
                .With.StructureMap()
                .And.ServiceLocator()
                .LookForTypesIn.ReferencedAssemblies().Including
                .Assembly(Assembly.GetAssembly(typeof(ApiContainer)))
                .Start();

            _container = (IContainer)Bootstrapper.Container;
            GlobalConfiguration.Configuration.DependencyResolver = new StructureMapDependencyResolver(_container);
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.IsoDateFormat;
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc;

            //This is just for intellisense when stepping through...
            var what = _container.WhatDoIHave();

            //Ensure our IoC is happy
            _container.AssertConfigurationIsValid();
            Log.Debug("IoC configuration is valid");

            Log.Info("API Started");
        }
    }
}
