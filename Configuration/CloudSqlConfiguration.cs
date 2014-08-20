using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Manufacturing.Api.Configuration
{
    public class CloudSqlConfiguration
    {
        public string ConfigSqlDatabaseConnectionString { get; set; }
        public string DataSqlDatabaseConnectionString { get; set; }
    }
}