using System;
using System.Collections.Generic;
using System.Threading;
using Manufacturing.Api.Data.Model;
using Manufacturing.Api.Hubs.Event;

namespace Manufacturing.Api.Hubs
{
    public class RandomDataSourceRecordHub
    {
        #region Fields

        private readonly DatasourceRecordHub _datasourceRecordHub;

        private readonly List<int> _messageId;

        private Timer _timer;

        #endregion

        #region Constructors

        public RandomDataSourceRecordHub(DatasourceRecordHub datasourceRecordHub)
        {
            _messageId = new List<int>();

            _datasourceRecordHub = datasourceRecordHub;
            _datasourceRecordHub.ConnectionChanged += datasourceRecordHubConnectionChanged;
            _timer = new Timer(tick, this, 0, 5000);
        }

        #endregion

        #region private

        private void datasourceRecordHubConnectionChanged(object sender, HubConnectionEventArgs e)
        {
            switch(e.HubConnectionType)
            {
                case HubConnectionType.Connected:
                    _messageId.Add(e.Id);
                    break;
                case HubConnectionType.Disconnected:
                    _messageId.Remove(e.Id);
                    break;
                case HubConnectionType.Reconnected:
                    if(!_messageId.Contains(e.Id))
                    {
                        _messageId.Add(e.Id);
                    }
                    break;
            }
        }

        private void tick(object state)
        {
            foreach(var messageId in _messageId)
            {
                var record = new DataRecord
                {
                    DatasourceId = 1,
                    EncodedDataType = 7,
                    Id = messageId,
                    IntervalSeconds = new Random().Next(0, 100),
                    Timestamp = DateTime.Now,
                    Value = BitConverter.GetBytes(new Random().Next(0, 100))
                };

                _datasourceRecordHub.Notify(record);
            }
        }

        #endregion
    }
}