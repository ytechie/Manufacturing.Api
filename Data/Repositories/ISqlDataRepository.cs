using Manufacturing.Api.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manufacturing.Api.Data.Repositories
{
    public interface ISqlDataRepository
    {
        IEnumerable<DataRecord> Find(int datasourceId, DateTime? startDateTime, DateTime? endDateTime, int maxRecords = 1000);
        DataRecord FindMostRecent(int datasourceId);

    }
}
