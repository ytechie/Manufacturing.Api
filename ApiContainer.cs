using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bootstrap.StructureMap;
using StructureMap;
using Bootstrap.Locator;
using Microsoft.ConventionConfiguration;
using StructureMap.Graph;
using StructureMap.Pipeline;

namespace Manufacturing.Api
{
    public class ApiContainer : IStructureMapRegistration
    {
        public void Register(IContainer container)
        {
            ConfigurationLoader.LoadConfigurations(container, ".\\Configuration\\", "{0}Configuration");

            container.Configure(x => x.Scan(y =>
            {
                y.TheCallingAssembly();
                y.SingleImplementationsOfInterface().OnAddedPluginTypes(z => z.LifecycleIs(new TransientLifecycle()));
            }));
        }

        public void Register(System.ComponentModel.IContainer container)
        {
        }
    }
}