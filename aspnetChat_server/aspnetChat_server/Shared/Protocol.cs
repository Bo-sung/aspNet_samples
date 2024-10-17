// 서버 클라 같이 사용하는 프로토콜 클래스.
// 파셜 클래스로 되어 있기때문에 서버에서는 해당 클래스의 파셜 클래스를 만들어 서버 핸들링을 하고
// 클라이언트에서는 해당 클래스의 파셜 클래스를 만들어 클라이언트 핸들링을 한다.
// 프로토콜 추가시 여기에서 추가후 전달할것

namespace aspnetChat_server.Protocols
{
    /// <summary>
    /// 서버 리절트 코드
    /// </summary>
    public enum RCode
    {
        SUCCESS = 0,
        FAIL = 1,
        ERROR = 2,
        PARAM_ERROR = 3,
    }

    /// <summary>
    /// 채팅 프로토콜 넘버
    /// </summary>
    public enum ChatProtocol
    {
        SendMessage = 0,
        RelayMessages = 1,
        RequestAllMessages = 2,
        ResponseAllMessages = 3,
    }

    /// <summary>
    /// 인증 프로토콜 넘버
    /// </summary>
    public enum AuthProtocol
    {
        Login = 0,
        Logout = 1,
        Register = 2,
        DuplicateCheck = 3,
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

            public bool IsSuccess()
            {
                return resultCode == RCode.SUCCESS;
            }

            public bool ContainsParam(int id)
            {
                switch (id)
                {
                    default:    return false;
                    case 1:     return param1 != null;
                    case 2:     return param2 != null;
                    case 3:     return param3 != null;
                    case 4:     return param4 != null;
                    case 5:     return param5 != null;
                    case 6:     return param6 != null;
                    case 7:     return param7 != null;
                    case 8:     return param8 != null;
                    case 9:     return param9 != null;
                    case 10:    return param10 != null;
                    case 11:    return param11 != null;
                    case 12:    return param12 != null;
                    case 13:    return param13 != null;
                    case 14:    return param14 != null;
                    case 15:    return param15 != null;
                }
            }

            /// <summary>
            /// 파라미터가 유효한지 체크
            /// </summary>
            /// <param name="id"></param>
            /// <param name="type"></param>
            /// <returns></returns>
            public bool ValidateParamType(int id, Type type)
            {
                switch (id)
                {
                    default:    return false;
                    case 1:     return param1    != null && param1?.type   == type;
                    case 2:     return param2    != null && param2?.type   == type;
                    case 3:     return param3    != null && param3?.type   == type;
                    case 4:     return param4    != null && param4?.type   == type;
                    case 5:     return param5    != null && param5?.type   == type;
                    case 6:     return param6    != null && param6?.type   == type;
                    case 7:     return param7    != null && param7?.type   == type;
                    case 8:     return param8    != null && param8?.type   == type;
                    case 9:     return param9    != null && param9?.type   == type;
                    case 10:    return param10   != null && param10?.type  == type;
                    case 11:    return param11   != null && param11?.type  == type;
                    case 12:    return param12   != null && param12?.type  == type;
                    case 13:    return param13   != null && param13?.type  == type;
                    case 14:    return param14   != null && param14?.type  == type;
                    case 15:    return param15   != null && param15?.type  == type;
                }
            }
        }
        public struct ParamStruct
        {
            public Type type;
            public object value;
        }

        // 에러 코드 전달용
        public static Param FailedParam = new Param() { resultCode = RCode.FAIL };
        public static Param ErrorParam = new Param() { resultCode = RCode.ERROR };
        public static Param ParamErrorParam = new Param() { resultCode = RCode.PARAM_ERROR };

    }
}