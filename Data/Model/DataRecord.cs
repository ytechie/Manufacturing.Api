using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Manufacturing.Api.Data.Model
{
    public class DataRecord
    {
        public int Id { get; set; }
        public int DatasourceId { get; set; }
        public DateTime Timestamp { get; set; }
        public int IntervalSeconds { get; set;}
        public byte[] Value { get; set; }
        public int EncodedDataType { get; set; }
    }
}