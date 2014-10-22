using System;
using System.Dynamic;
using System.Reactive;
using System.Reflection;
using log4net;
using Manufacturing.Framework.Datasource;
using Manufacturing.Framework.Dto;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.ServiceBus.Messaging;

namespace Manufacturing.Api.RealtimeDataStream
{
    [HubName("DatasourceRecord")]
    public class DatasourceRecordHub : Hub
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

        public void Notify(DatasourceRecord message)
        {
            Notify(Clients, message);
        }

        public static void Notify(IHubConnectionContext<dynamic> clients, DatasourceRecord message)
        {
            //Filter out old data, we're only interested in real-tme
            if (message.Timestamp < DateTime.UtcNow.AddMinutes(-1))
                return;

            dynamic dataRecord = new ExpandoObject();
            dataRecord.DatasourceId = message.DatasourceId;
            dataRecord.EncodedDataType = message.EncodedDataType;
            dataRecord.IntervalSeconds = message.IntervalSeconds;
            dataRecord.Timestamp = message.Timestamp;

            switch (message.DataType)
            {
                case DatasourceRecord.DataTypeEnum.Decimal:
                    dataRecord.Value = message.GetDecimalValue();
                    break;
                case DatasourceRecord.DataTypeEnum.Integer:
                    dataRecord.Value = message.GetIntValue();
                    break;
                case DatasourceRecord.DataTypeEnum.Double:
                    dataRecord.Value = message.GetDoubleValue();
                    break;
            }
            
            var group = clients.Group(GroupLabelPrefix + message.DatasourceId);
            if (group != null)
            {
                group.newRecord(dataRecord);
            }
        }
    }
}