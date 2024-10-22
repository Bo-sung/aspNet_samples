using aspnetChat_server.Protocols;
using MySql.Data.MySqlClient;
using System.Reflection;
using System.Reflection.PortableExecutable;

namespace aspnetChat_server.DB
{
    public class DBService
    {
        protected string _connectionString;

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

    public sealed class DB<T> : DBService where T : class, new()
    {
        public DB(string _connectionString) : base(_connectionString) { }

        public (RCode, List<T>) RequestSelectData(string _target_table, string _target_param = "")
        {
            if (_target_param == "")
                return GetData($"SELECT * FROM {_target_table}");
            else
                return GetData($"SELECT {_target_param} FROM {_target_table}");
        }

        public (RCode, List<T>) RequestSelectData(string _target_table, int _limit, string _target_param = "")
        {
            if (_target_param == "")
                return GetData($"SELECT * FROM {_target_table} LIMIT {_limit}");
            else
                return GetData($"SELECT {_target_param} FROM {_target_table} LIMIT {_limit}");
        }

        public (RCode, T) RequestSingleSelectData(string _target_table, string _target_param = "")
        {
            if (_target_param == "")
                return SelectOneData($"SELECT * FROM {_target_table} LIMIT 1;");
            else
                return SelectOneData($"SELECT {_target_param} FROM {_target_table} LIMIT 1;");
        }

        public RCode RequestInsertData(string _target_table, string _target_param, string _target_value)
        {
            return InsertData($"INSERT INTO {_target_table} ({_target_param}) VALUES ({_target_value})");
        }

        public RCode RequestUpdateData(string _target_table, string _target_param, string _target_value, string _where_param, string _where_value)
        {
            return UpdateData($"UPDATE {_target_table} SET {_target_param} = {_target_value} WHERE {_where_param} = {_where_value}");
        }

        public RCode RequestDeleteData(string _target_table, string _where_param, string _where_value)
        {
            return DeleteData($"DELETE FROM {_target_table} WHERE {_where_param} = {_where_value}");
        }

        /// <summary>
        /// DB Select 직접 호출
        /// </summary>
        /// <param name="_query"></param>
        /// <returns></returns>
        protected (RCode, List<T>) GetData(string _query)
        {
            List<T> list = new List<T>();
            using (MySqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(_query, conn);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // 리스트에 추가
                            list.Add(BuildValue(reader));
                        }
                    }
                    conn.Close();
                }
                catch (MySqlException e)
                {
                    return (RCode.FAIL, null);
                }
            }
            return (RCode.SUCCESS, list);
        }
        /// <summary>
        /// DB Select 직접 호출
        /// </summary>
        /// <param name="_query"></param>
        /// <returns></returns>
        protected (RCode, T) SelectOneData(string _query)
        {
            T result = null;
            using (MySqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(_query, conn);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        reader.Read();
                        // 리스트에 추가
                        result = BuildValue(reader);
                    }
                    conn.Close();
                }
                catch (MySqlException e)
                {
                    return (RCode.FAIL, result);
                }
            }
            return (RCode.SUCCESS, result);
        }

        /// <summary>
        /// DB 직접 호출. 단순히 DB 리턴 없는경우만 사용
        /// </summary>
        /// <param name="_query"></param>
        /// <returns></returns>
        protected RCode CallDB(string _query)
        {
            using (MySqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(_query, conn);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (cmd.ExecuteNonQuery() == 0)
                            return RCode.FAIL;
                    }
                    conn.Close();
                }
                catch (MySqlException e)
                {
                    return RCode.FAIL;
                }
            }

            return RCode.SUCCESS;
        }

        /// <summary>
        /// DB Insert 직접 호출
        /// </summary>
        /// <param name="_query"></param>
        /// <returns></returns>
        public RCode InsertData(string _query)
        {
            return CallDB(_query);
        }

        /// <summary>
        /// DB Update 직접 호출
        /// </summary>
        /// <param name="_query"></param>
        /// <returns></returns>
        public RCode UpdateData(string _query)
        {
            return CallDB(_query);
        }

        /// <summary>
        /// DB Delete 직접 호출
        /// </summary>
        /// <param name="_query"></param>
        /// <returns></returns>
        public RCode DeleteData(string _query)
        {
            return CallDB(_query);
        }

        /// <summary>
        /// 리플렉션으로 데이터 추가
        /// </summary>
        /// <param name="_reader"></param>
        /// <returns></returns>
        protected T BuildValue(MySqlDataReader _reader)
        {
            T temp = new T();
            Type type = temp.GetType();
            PropertyInfo[] properties = type.GetProperties();
            foreach (PropertyInfo property in properties)
            {
                property.SetValue(temp, _reader[property.Name]);
            }
            return temp;
        }
    }
    //public class DBChatMessage : DBService
    //{
    //    public DBChatMessage(string _connectionString) : base(_connectionString) { }

    //    public List<ChatMessage> GetData()
    //    {
    //        List<ChatMessage> list = new List<ChatMessage>();
    //        string query = "SELECT * FROM chatmessage";
    //        // using문 사용 이유
    //        // https://magpienote.tistory.com/65
    //        // https://docs.microsoft.com/ko-kr/dotnet/csharp/language-reference/keywords/using-statement
    //        // 인스턴스 사용 후 자동 할당 해제됨
    //        using (MySqlConnection conn = GetConnection())
    //        {
    //            conn.Open();
    //            MySqlCommand cmd = new MySqlCommand(query, conn);
    //            using (MySqlDataReader reader = cmd.ExecuteReader())
    //            {
    //                while (reader.Read())
    //                {
    //                    list.Add(new ChatMessage()
    //                    {
    //                        id = reader.GetInt32("id"),
    //                        user = reader.GetString("user"),
    //                        message = reader.GetString("message"),
    //                        time = reader.GetDateTime("time")
    //                    });
    //                }
    //            }
    //        }
    //        return list;
    //    }
    //}
}
