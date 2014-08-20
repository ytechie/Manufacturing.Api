using Manufacturing.Api.Data.Model;
using Manufacturing.Api.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Web.Http;

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
        // /api/Data?datasourceId=1&startDateTime=2014-03-02T12%3A34%3A56&endDateTime=2014-03-02T23%3A34%3A56
        public IEnumerable<DataRecord> Get(int datasourceId, DateTime? startDateTime = null, DateTime? endDateTime = null)
        {
            var result = _repos.Find(datasourceId, startDateTime, endDateTime);
            return result;
        }
    }
}
