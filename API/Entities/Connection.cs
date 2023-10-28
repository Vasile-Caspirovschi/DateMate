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
            Username = usename;
        }

        public string ConnectionId { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
    }
}