using Manufacturing.Api.Configuration;
using Manufacturing.Api.Data.Model;
using Manufacturing.Framework.Datasource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Manufacturing.Api.Data.Repositories
{
    public class SqlDataRepository : ISqlDataRepository
    {
        private DataEntities _entities;

        public SqlDataRepository(CloudSqlConfiguration cfg)
        {
            _entities = new DataEntities(cfg.DataSqlDatabaseConnectionString);
        }

        // Adding a default max record specification here to prevent potentially inadvertent giant queries
        public IEnumerable<DataRecord> Find(int datasourceId, DateTime? startDateTime, DateTime? endDateTime, int maxRecords = 1000)
        {
            var qry = _entities.DataRecords.Where(x => x.DatasourceId == datasourceId);
            if (startDateTime.HasValue)
            {
                qry = qry.Where(x => x.Timestamp > startDateTime.Value);
            }
            if (endDateTime.HasValue)
            {
                qry = qry.Where(x => x.Timestamp <= endDateTime.Value);
            }

            var result = qry.Take(maxRecords).ToList();
            return result;
        }

        // Return the most recent record for the given Data Source Id
        public DataRecord FindMostRecent(int datasourceId)
        {
            string qryTxt = "select * from raw where id = (" +
                    "select top 1 id from dbo.raw where datasourceid = 1 order by id desc)";

            var qry = _entities.DataRecords
                .Where(x => x.DatasourceId == datasourceId)
                .OrderByDescending(x => x.Id)
                .FirstOrDefault();

            var record = qry;
            return record;
        }
    }
}