using aspnetChat_server.DB;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using aspnetChat_server.Protocols.Chat;

namespace aspnetChat_server.Protocols
{
    public class ChatHub : Hub
    {
        public static string URL_HEADER = "/chatHub";
        /// <summary>
        /// 모든 메시지 가져오기
        /// </summary>
        public Dictionary<string, ChatMessage> GetAllMessages()
        {
            // DB에서 모든 메시지를 가져옴
            var messages = DBManager.Instance.DBMessage.GetAllMessages();
            var messages2 = DBManager.Instance.DBChat.RequestSelectData("`chatmessage`");
            if (messages == null)
                return null;
            Dictionary<string, ChatMessage> result = new Dictionary<string, ChatMessage>();
            foreach (var kvp in messages2.Item2)
            {
                result.Add(kvp.id.ToString(), new ChatMessage() { user = kvp.user, message = kvp.message });
            }
            // 메시지가 없으면 빈 문자열 반환
            if (result == null)
                return null;
            // 메시지를 JSON 문자열로 반환
            return result;
        }

        /// <summary>
        /// 클라이언트에서 서버로 메시지 전송
        /// </summary>
        /// <param name="user">유저</param>
        /// <param name="message">메시지</param>
        public async Task SendMessage(string param)
        {
            var temp = Protocol.DeserializeParam(param);
            if (temp == null)
            {
                // 파라미터 에러 전송
                await Protocol.RelayMessages.SendCaller(Clients, Protocol.FailedParam);
                return;
            }
            // 파라미터 1번 유효성 체크
            if (!temp.ValidateParamType(1, typeof(ChatMessage)))
            {
                // 파라미터 에러 전송
                await Protocol.RelayMessages.SendCaller(Clients, Protocol.ParamErrorParam);
                return;
            }
            ChatMessage messageStruct = (ChatMessage)temp.param1?.value;

            // DB에 메시지 추가
            DBManager.Instance.DBMessage.AddMessage(messageStruct.message, messageStruct.user);

            // 받은 파라미터 그대로 전달. (위에서 검증했으니까 그대로 보내도 될듯?)
            // 모든 클라이언트에게 메시지 전송
            await Protocol.RelayMessages.SendAll(Clients, temp);
        }

        /// <summary>
        /// 클라이언트에서 서버로 모든 메시지 요청
        /// </summary>
        public async Task RequestAllMessages()
        {
            // 값 가져오기
            Dictionary<string, ChatMessage> messageDic = GetAllMessages();
            // 파라미터 세팅
            Protocol.Param param = new Protocol.Param()
            {
                // 성공처리
                resultCode = RCode.SUCCESS,
                // 파라미터 1번에 메시지 딕셔너리 넣기
                param1 = new Protocol.ParamStruct()
                {
                    type = messageDic.GetType(),
                    value = messageDic
                }
            };

            // 요청한 클라이언트에게 메시지 딕셔너리 전송
            await Protocol.RequestAllMessages.SendCaller(Clients, param);
        }
    }
}