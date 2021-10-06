
namespace LocationSearch.Api.Settings
{
    public class MySqlSettings
    {
        public string Host { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string Database { get; set; }
        public int Port { get; set; }
        public string ConnectionString
        {
            get
            {
                return $"Server={Host};Port={Port};Database={Database};Uid={User};Pwd={Password};";
            }
        }
    }
}
