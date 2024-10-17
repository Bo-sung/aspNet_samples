// 서버 클라 같이 사용하는 프로토콜 클래스.
// 파셜 클래스로 되어 있기때문에 서버에서는 해당 클래스의 파셜 클래스를 만들어 서버 핸들링을 하고
// 클라이언트에서는 해당 클래스의 파셜 클래스를 만들어 클라이언트 핸들링을 한다.
// 프로토콜 추가시 여기에서 추가후 전달할것

using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace aspnetChat_server.Protocols
{
    public enum RCode
    {
        SUCCESS = 0,
        FAIL = 1,
        ERROR = 2
    }

    public enum ChatProtocol
    {
        SendMessage = 0,
        RelayMessages = 1,
        RequestAllMessages = 2,
        ResponseAllMessages = 3,
    }

    public partial class Protocol2
    {
        public enum ProtocolType
        {
            empty = -1,
            auth = 0,
            chat = 1,
        }

        private string m_protocolStr = "";
        private int m_protoNum = 0;
        private ProtocolType m_protoType = ProtocolType.empty;
        private bool m_isServerOnly = false;

        /// <summary>
        /// 클라 -> 서버 메세지 전송
        /// </summary>
        public static Protocol2 SendMessage = new Protocol2(ProtocolType.chat, (int)ChatProtocol.SendMessage, "SendMessage");
        /// <summary>
        /// 서버 -> 접속중인 모든 클라 메시지 전송
        /// </summary>
        public static Protocol2 RelayMessages = new Protocol2(ProtocolType.chat, (int)ChatProtocol.RelayMessages, "RelayMessages",true);
        /// <summary>
        /// 클라 -> 서버 모든 메시지 요청 및 서버 -> 클라 모든 메시지 전송
        /// </summary>
        public static Protocol2 RequestAllMessages = new Protocol2(ProtocolType.chat, (int)ChatProtocol.RequestAllMessages, "RequestAllMessages");

        private Protocol2(ProtocolType _type, int _protoNum, string _protocolStr, bool isServer = false)
        {
            this.m_protoType = _type;
            this.m_protoNum = _protoNum;
            this.m_protocolStr = _protocolStr;
            m_isServerOnly = isServer;
        }

        public class Param
        {
            public RCode resultCode;
            public ParamStruct? param1;
            public ParamStruct? param2;
            public ParamStruct? param3;
            public ParamStruct? param4;
            public ParamStruct? param5;
            public ParamStruct? param6;
            public ParamStruct? param7;
            public ParamStruct? param8;
            public ParamStruct? param9;
            public ParamStruct? param10;
            public ParamStruct? param11;
            public ParamStruct? param12;
            public ParamStruct? param13;
            public ParamStruct? param14;
            public ParamStruct? param15;
        }

        public struct ParamStruct
        {
            public Type type;
            public object value;
        }
    }

    // Server Only
    public partial class Protocol2
    {
        public async Task SendAll(IHubCallerClients clients, object obj)
        {
            if (clients == null)
                return;
            if (obj == null)
                return;
            string paramStr = JsonConvert.SerializeObject(obj);
            await clients.All.SendAsync(m_protocolStr, paramStr);
        }

        public async Task SendCaller(IHubCallerClients clients, object obj)
        {
            if (clients == null)
                return;
            if (obj == null)
                return;
            string paramStr = JsonConvert.SerializeObject(obj);
            await clients.Caller.SendAsync(m_protocolStr, paramStr);
        }
    }


    public partial class Protocol
    {
        public abstract class BaseProtocol2
        {
            public static string ProtoStr = "";
            public abstract class Param
            {
            }

            public abstract class Result
            {
                public required RCode resultCode;
            }
        }

        public partial class Chat
        {
            public static string URL_HEADER = "/chatHub";
            public struct ChatMessage
            {
                public string user;
                public string message;
            }

            public partial class ServerSide
            {
                public sealed class ReceiveMessage : BaseProtocol2
                {
                    public static new string ProtoStr = "ReceiveMessage";
                    public new class Param : BaseProtocol2.Param
                    {
                        public ChatMessage message;
                    }

                    public new class Result : BaseProtocol2.Result
                    {
                        public ChatMessage message;
                    }
                }

                public sealed class ResponseAllMessages : BaseProtocol2
                {
                    public static new string ProtoStr = "ResponseAllMessages";
                    public new class Param : BaseProtocol2.Param
                    {

                    }

                    public new class Result : BaseProtocol2.Result
                    {
                        public Dictionary<string, ChatMessage> messageDic;
                    }
                }
            }

            public partial class ClientSide
            {
                public sealed partial class SendMessage : BaseProtocol2
                {
                    public static new string ProtoStr = "SendMessage";
                    public new class Param : BaseProtocol2.Param
                    {
                        public ChatMessage message;
                    }

                    public new class Result : BaseProtocol2.Result
                    {
                        public ChatMessage message;
                    }
                }

                public sealed partial class RequestAllMessages : BaseProtocol2
                {
                    public static new string ProtoStr = "RequestAllMessages";
                    public new class Param : BaseProtocol2.Param
                    {

                    }

                    public new class Result : BaseProtocol2.Result
                    {
                        public Dictionary<string, ChatMessage> messageDic;
                    }
                }
            }
        }
    }
}