using System.Reactive;
using System.Reflection;
using log4net;
using Manufacturing.Framework.Dto;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

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

        public void Notify(DatasourceRecord message)
        {
            Notify(Clients, message);
        }

        public static void Notify(IHubConnectionContext<dynamic> clients, DatasourceRecord message)
        {
            var dataRecord = new
            {
                message.DatasourceId,
                message.EncodedDataType,
                message.IntervalSeconds,
                message.Timestamp,
                message.Value,
            };
            
            var group = clients.Group(GroupLabelPrefix + message.DatasourceId);
            if (group != null)
            {
                group.newRecord(dataRecord);
            }
        }
    }
}