namespace API.Entities
{
    public class Connection
    {
        public Connection()
        {
        }

        public Connection(string connectionId, string usename)
        {
            ConnectionId = connectionId;
            Usename = usename;
        }

        public string ConnectionId { get; set; } = string.Empty;
        public string Usename { get; set; } = string.Empty;
    }
}