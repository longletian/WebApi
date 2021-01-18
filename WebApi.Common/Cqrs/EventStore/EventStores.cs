using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using Newtonsoft.Json;

namespace WebApi.Common
{
    public class EventStores : IEventStores
    {
        private readonly IEventStoreConnection connection;
        private const string metadata = "{}";
        public EventStores()
        {
            connection= EventStoreConnection.Create(
                new Uri(AppSetting.GetConnStrings("EventStoreCon"))
            );
            connection.ConnectAsync();
        }
        
        public async Task EventSave(Event @event, CancellationToken cancellationToken = default)
        {
            var serializedData = JsonConvert.SerializeObject(@event);
            var eventPayload = new EventData(
              eventId:@event.AggregateId,
              type: @event.MessageType,
              isJson: true,
              data: Encoding.UTF8.GetBytes(serializedData),
              metadata: Encoding.UTF8.GetBytes(metadata)
            );
            await connection.AppendToStreamAsync(@event.EventName, ExpectedVersion.Any, eventPayload);
        }

        public async Task<List<ResolvedEvent>> GetEvents(Guid aggregateId)
        {
            var allEvents = new List<ResolvedEvent>();
            AllEventsSlice currentSlice;
            var nextSliceStart = Position.Start;
            do
            {
                currentSlice = await connection.ReadAllEventsForwardAsync(
                    nextSliceStart,
                    200, false
                );

                nextSliceStart = currentSlice.NextPosition;

                allEvents.AddRange(currentSlice.Events);
            } while (!currentSlice.IsEndOfStream);
            return allEvents;
        }

        public IEventStoreConnection GetEventStoreConnection()
        {
            return this.connection;
        }
    }
}
