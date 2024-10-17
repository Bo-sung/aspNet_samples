// 서버 클라 같이 사용하는 프로토콜 클래스.
// 파셜 클래스로 되어 있기때문에 서버에서는 해당 클래스의 파셜 클래스를 만들어 서버 핸들링을 하고
// 클라이언트에서는 해당 클래스의 파셜 클래스를 만들어 클라이언트 핸들링을 한다.
// 프로토콜 추가시 여기에서 추가후 전달할것

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

    public partial class Protocol
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
        public static Protocol SendMessage = new Protocol(ProtocolType.chat, (int)ChatProtocol.SendMessage, "Chat/SendMessage");
        /// <summary>
        /// 서버 -> 접속중인 모든 클라 메시지 전송
        /// </summary>
        public static Protocol RelayMessages = new Protocol(ProtocolType.chat, (int)ChatProtocol.RelayMessages, "Chat/RelayMessages", true);
        /// <summary>
        /// 클라 -> 서버 모든 메시지 요청 및 서버 -> 클라 모든 메시지 전송
        /// </summary>
        public static Protocol RequestAllMessages = new Protocol(ProtocolType.chat, (int)ChatProtocol.RequestAllMessages, "Chat/RequestAllMessages");

        private Protocol(ProtocolType _type, int _protoNum, string _protocolStr, bool isServer = false)
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
}