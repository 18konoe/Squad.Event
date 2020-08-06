namespace SquadEvent.Server.Model
{
    public class EventsStoreDatabaseSettings : IEventsStoreDatabaseSettings
    {
        public string EventsCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IEventsStoreDatabaseSettings
    {
        string EventsCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}