using System.Reflection;
using System.Threading.Tasks;
using log4net;
using Manufacturing.Api.Data.Model;
using Manufacturing.Api.Hubs.ChannelResolvers;
using Manufacturing.Api.Hubs.Event;
using Manufacturing.Framework.Dto;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace Manufacturing.Api.Hubs
{
    [HubName("DatasourceRecord")]
    public class DatasourceRecordHub : Hub
    {
        #region Fields

        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IChannelResolver<DatasourceRecord> _channelResolver;

        private RandomDataSourceRecordHub _randomDataGenerator;

        public delegate void ConnectionChangedEventHandler(object sender, HubConnectionEventArgs e);

        public event ConnectionChangedEventHandler ConnectionChanged;

        #endregion

        #region Constructors

        public DatasourceRecordHub() : this(new DatasourceRecordChannelResolver<DatasourceRecord>()) { }

        internal DatasourceRecordHub(IChannelResolver<DatasourceRecord> channelResolver)
        {
            _channelResolver = channelResolver;
            _randomDataGenerator= new RandomDataSourceRecordHub(this);
        }

        #endregion

        #region public

        public void Notify(DatasourceRecord message)
        {
            // TODO: FILTER! Might be able to do group subscriptions instead of channel resolvers
            //var id = _channelResolver.GetChannelId(message);

            //_log.DebugFormat("Received a message from {0} with an ID of {1}", message.DatasourceId, id);

            //if(id == null)
            //{
            //    // log or something
            //    return;
            //}

            //Clients.Client(id).notify(message);

            // Convert to DataRecord (type mismatch???)
            var dataRecord = new DataRecord
            {
                DatasourceId = message.DatasourceId,
                EncodedDataType = message.EncodedDataType,
                IntervalSeconds = message.IntervalSeconds,
                Timestamp = message.Timestamp,
                Value = message.Value,
                //Id = message.Id //COMMENTED OUT TO FIX THE BUILD
            };

            Clients.All.notify(dataRecord);
        }

        public void Notify(DataRecord message)
        {
            Clients.All.notify(message);
        }

        public void Register(int id)
        {
            _channelResolver.SetChannelId(id, Context.ConnectionId);
            ConnectionChanged(this, new HubConnectionEventArgs{HubConnectionType = HubConnectionType.Connected, Id = id});
        }

        #region Overrides of HubBase

        /// <summary>
        /// Called when the connection connects to this hub instance.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Threading.Tasks.Task"/>
        /// </returns>
        public override Task OnConnected()
        {
            var t = 1;
            return base.OnConnected();
        }

        #endregion

        #endregion
    }
}