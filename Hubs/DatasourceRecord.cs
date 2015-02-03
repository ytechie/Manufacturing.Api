using System;
using System.Dynamic;
using System.Reflection;
using log4net;
using Manufacturing.Framework.Datasource;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace Manufacturing.Api.Hubs
{
    [HubName("DatasourceRecord")]
    public class DatasourceRecord : Hub
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public const string GroupLabelPrefix = "Datasource_";

        public void Register(int datasourceId)
        {
            Log.DebugFormat("Registering connection {0} for datasource {1}", Context.ConnectionId, datasourceId);
            
            Groups.Add(Context.ConnectionId, GroupLabelPrefix + datasourceId);
        }

        public void Unregister(int datasourceId)
        {
            Log.DebugFormat("Unregistering connection {0} for datasource {1}", Context.ConnectionId, datasourceId);

            Groups.Remove(Context.ConnectionId, GroupLabelPrefix + datasourceId);
        }

        public static void Notify(Framework.Dto.DatasourceRecord message)
        {
            //Filter out old data, we're only interested in real-tme
            if (message.Timestamp < DateTime.UtcNow.AddMinutes(-1))
                return;

            var context = GlobalHost.ConnectionManager.GetHubContext<DatasourceRecord>();

            dynamic dataRecord = new ExpandoObject();
            dataRecord.DatasourceId = message.DatasourceId;
            dataRecord.EncodedDataType = message.EncodedDataType;
            dataRecord.IntervalSeconds = message.IntervalSeconds;
            dataRecord.Timestamp = message.Timestamp;

            switch (message.DataType)
            {
                case Framework.Dto.DatasourceRecord.DataTypeEnum.Decimal:
                    dataRecord.Value = message.GetDecimalValue();
                    break;
                case Framework.Dto.DatasourceRecord.DataTypeEnum.Integer:
                    dataRecord.Value = message.GetIntValue();
                    break;
                case Framework.Dto.DatasourceRecord.DataTypeEnum.Double:
                    dataRecord.Value = message.GetDoubleValue();
                    break;
            }
            
            var group = context.Clients.Group(GroupLabelPrefix + message.DatasourceId);
            if (group != null)
            {
                group.newRecord(dataRecord);
            }
        }
    }
}