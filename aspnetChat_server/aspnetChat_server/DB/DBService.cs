using MySql.Data.MySqlClient;

namespace aspnetChat_server.DB
{
    public class DBService
    {
        protected string _connectionString = "";

        public string ConnectionString { get => _connectionString; }

        public DBService(string _connectionString)
        {
            this._connectionString = _connectionString;
        }

        protected MySqlConnection GetConnection()
        {
            return new MySqlConnection(_connectionString);
        }
    }

    public class DBChatMessage : DBService
    {
        public DBChatMessage(string _connectionString) : base(_connectionString) { }
    }

}
