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
            if (messages == null)
                return null;
            Dictionary<string, ChatMessage> result = new Dictionary<string, ChatMessage>();
            foreach (var kvp in messages)
            {
                result.Add(kvp.Key, kvp.Value.message);
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
            //var temp = JsonConvert.DeserializeObject<Protocol2.Param>(param);
            if (temp == null)
                return;
            // 파라미터 1번 있는지 체크
            if (temp.param1 == null)
                return;
            // 파라미터 1번이 ChatMessage인지 체크
            if (temp.param1?.type != typeof(ChatMessage))
                return;
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
            Dictionary<string, ChatMessage> messageDic = GetAllMessages();
            Protocol.Param param = new Protocol.Param()
            {
                resultCode = RCode.SUCCESS,
                param1 = new Protocol.ParamStruct()
                {
                    type = messageDic.GetType(),
                    value = messageDic
                }
            };

            await Protocol.RequestAllMessages.SendCaller(Clients, param);
        }
    }
}