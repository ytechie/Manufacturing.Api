using Manufacturing.Api.Data.Model;
using Manufacturing.Api.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Web.Http;
using Manufacturing.Framework.Dto;
using Microsoft.AspNet.SignalR;
using DatasourceRecord = Manufacturing.Api.Hubs.DatasourceRecord;

namespace Manufacturing.Api.Controllers
{
    public class DataController : ApiController
    {
        private readonly ISqlDataRepository _repos;

        public DataController(ISqlDataRepository repos)
        {
            _repos = repos;
        }

        // DateTimes can be specified in qs in ISO 8601 format (url encoded), e.g., 
        // /api/Data?datasourceId=1&startDateTime=2014-03-02T12%3A34%3A56
        public IEnumerable<DataRecord> Get(int datasourceId, DateTime startDateTime)
        {
            var result = _repos.Find(datasourceId, startDateTime, null);
            return result;
        }

        // /api/Data?datasourceId=1&startDateTime=2014-03-02T12%3A34%3A56&endDateTime=2014-03-02T23%3A34%3A56
        public IEnumerable<DataRecord> Get(int datasourceId, DateTime startDateTime, DateTime endDateTime)
        {
            var result = _repos.Find(datasourceId, startDateTime, endDateTime);
            return result;
        }

        // Only returns the most recent record for this data source id
        // /api/Data?datasourceId=1 
        public DataRecord Get(int datasourceId)
        {
            var result = _repos.FindMostRecent(datasourceId);
            return result;
        }

        public void Post([FromBody] List<Framework.Dto.DatasourceRecord> records)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<DatasourceRecord>();
            
            foreach (var record in records)
            {
                DatasourceRecord.Notify(context.Clients, record);
            }
        }
    }
}
