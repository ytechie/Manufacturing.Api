using System;

namespace Manufacturing.Api.Hubs.Event
{
    public class HubConnectionEventArgs : EventArgs
    {
        public HubConnectionType HubConnectionType;

        public int Id;
    }
}