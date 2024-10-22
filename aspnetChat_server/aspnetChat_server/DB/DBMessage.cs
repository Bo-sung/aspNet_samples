using System.Reflection.Metadata;
using System.Text.Json;

namespace aspnetChat_server.DB
{
    using DBDicType = Dictionary<string, DBMessage.Element>;

    public class ChatMessage_data
    {
        public int id { get; set; }
        public string user { get; set; }
        public string message { get; set; }
        public DateTime time { get; set; }
    }

    public class DBMessage
    {
        public class Element
        {
            public Protocols.Chat.ChatMessage message;
            public DateTime time;

            public Element(string user, string message)
            {
                this.message.user = user;
                this.message.message = message;
                this.time = DateTime.Now;
            }
        }
        private readonly DBDicType _dic_Messages = new DBDicType();

        public DBMessage() { }
        public DBMessage(string _json)
        {
            DBDicType? prev = JsonSerializer.Deserialize<DBDicType>(_json);
            if (prev != null)
                _dic_Messages = prev;
        }

        public DBDicType Dic_Messages { get => _dic_Messages; }

        /// <summary>
        /// 모든 메시지 가져오기
        /// </summary>
        /// <returns></returns>
        public DBDicType GetAllMessages()
        {
            return _dic_Messages;
        }

        public Element GetMessage(string id)
        {
            if (_dic_Messages == null || !_dic_Messages.ContainsKey(id))
                return null;
            return _dic_Messages[id];
        }

        public void AddMessage(string user, string message)
        {
            _dic_Messages.Add(_dic_Messages.Count.ToString(), new Element(user, message));
        }

        public string FindMessage_key(string user, string message)
        {
            foreach (KeyValuePair<string, Element> kvp in _dic_Messages)
            {
                if (kvp.Value.message.user == user && kvp.Value.message.message == message)
                    return kvp.Key;
            }
            return "";
        }

        public Element[] FindMessages(string user)
        {
            List<Element> list = new List<Element>();
            foreach (KeyValuePair<string, Element> kvp in _dic_Messages)
            {
                if (kvp.Value.message.user == user)
                    list.Add(kvp.Value);
            }
            return list.ToArray();
        }
    }
}
