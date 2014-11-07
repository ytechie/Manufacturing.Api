using System;
using System.Threading;
using Manufacturing.Framework.Alarms;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace Manufacturing.Api.Hubs
{
    [HubName("Alarms")]
    public class Alarms : Hub
    {
        private static Timer _randomAlarmGenerator;
        private static int _messageNumber;

        public void Start()
        {
            //Until we have a real alarm system, we'll just generate simulated alarms here
            if (_randomAlarmGenerator == null)
            {
                _randomAlarmGenerator = new Timer(state => Notify(new Alarm
                {
                    Timestamp = DateTime.UtcNow,
                    Message = "Alarm Message " + _messageNumber++
                }), null, TimeSpan.Zero, TimeSpan.FromSeconds(1));
            }
        }

        public void Notify(Alarm alarm)
        {
            Notify(Clients, alarm);
        }

        //This allows us to send data from a static context
        public static void Notify(IHubConnectionContext<dynamic> clients, Alarm alarm)
        {
            //Filter out old data, we're only interested in real-time
            if (alarm.Timestamp < DateTime.UtcNow.AddMinutes(-1))
                return;

            clients.All.newAlarm(alarm);
        }
    }
}