using MongoDB.Driver;
using SquadEvent.Shared.Models;

namespace SquadEvent.Server.Model
{
    public class EventCollectionProvider : IMongoCollectionProvider<EventModel>
    {
        private readonly IMongoCollection<EventModel> _events;
        public EventCollectionProvider(IEventsStoreDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _events = database.GetCollection<EventModel>(settings.EventsCollectionName);
        }
        public IMongoCollection<EventModel> GetCollection()
        {
            return _events;
        }
    }

    public interface IMongoCollectionProvider<T>
    {
        IMongoCollection<T> GetCollection();
    }
}