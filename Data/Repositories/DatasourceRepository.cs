using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Manufacturing.Api.Data.Model;
using Manufacturing.Api.Configuration;
using Manufacturing.Framework.Dto;

namespace Manufacturing.Api.Data.Repositories
{
    public class DatasourceRepository : IDatasourceRepository
    {
        private ConfigEntities _entities;

        public DatasourceRepository(CloudSqlConfiguration cfg)
        {
            _entities = new ConfigEntities(cfg.ConfigSqlDatabaseConnectionString);
        }

        public IEnumerable<DatasourceConfiguration> GetAll()
        {
            return _entities.Datasources.ToList();
        }
    }
}