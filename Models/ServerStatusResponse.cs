namespace PoolServer.Models
{
    public class ServerStatusResponse
    {
        public string Server { get; set; }
        public string Version { get; set; }
        public bool Operational { get; set; }
    }
}