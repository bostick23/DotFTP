namespace DotFTP.Client
{
    public class BaseFtpClient
    {
        public string Host { get; protected set; }
        public int? Port { get; protected set; }
        public string Username { get; protected set; }
        public string Password { get; protected set; }
    }
}
