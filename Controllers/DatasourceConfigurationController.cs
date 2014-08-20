using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Manufacturing.Api.Data.Repositories;
using Manufacturing.Api.Data.Model;
using Manufacturing.Framework.Dto;

namespace Manufacturing.Api.Controllers
{
    public class DatasourceConfigurationController : ApiController
    {
        private IDatasourceRepository _repos;

        public DatasourceConfigurationController(IDatasourceRepository repos)
        {
            _repos = repos;
        }

        public IEnumerable<DatasourceConfiguration> Get() {
            var datasources = _repos.GetAll().ToList();
            return datasources;
        }
    }
}
