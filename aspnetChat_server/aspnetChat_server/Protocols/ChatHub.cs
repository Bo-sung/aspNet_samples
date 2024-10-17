using aspnetChat_server.DB;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System.Reflection.Metadata;
using static aspnetChat_server.Protocols.Protocol.Chat;

namespace aspnetChat_server.Protocols
{
    public class ChatHub : Hub
    {
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
            var temp = JsonConvert.DeserializeObject<Protocol.Chat.ClientSide.SendMessage.Param>(param);
            if (temp == null)
                return;

            // DB에 메시지 추가
            DBManager.Instance.DBMessage.AddMessage(temp.message.user, temp.message.message);
            Protocol.Chat.ClientSide.SendMessage.Result temp2 = new Protocol.Chat.ClientSide.SendMessage.Result()
            {
                resultCode = RCode.SUCCESS,
                message = temp.message
            };

            Protocol2.Param temp3 = new Protocol2.Param()
            {
                resultCode = RCode.SUCCESS,
                param1 = new Protocol2.ParamStruct()
                {
                    type = temp.message.GetType(),
                    value = temp.message
                }
            };

            // 모든 클라이언트에게 메시지 전송
            await Protocol2.RelayMessages.SendAll(Clients, temp2);

            //// 모든 클라이언트에게 메시지 전송
            //await Clients.All.SendAsync(
            //    Protocol.Chat.ClientSide.SendMessage.ProtoStr,
            //    JsonConvert.SerializeObject(temp2));

        }

        /// <summary>
        /// 클라이언트에서 서버로 모든 메시지 요청
        /// </summary>
        public async Task RequestAllMessages()
        {
            Protocol.Chat.ClientSide.RequestAllMessages.Result temp = new Protocol.Chat.ClientSide.RequestAllMessages.Result()
            {
                resultCode = RCode.SUCCESS,
                messageDic = GetAllMessages()
            };


            Dictionary<string, ChatMessage> messageDic = GetAllMessages();
            Protocol2.Param temp3 = new Protocol2.Param()
            {
                resultCode = RCode.SUCCESS,
                param1 = new Protocol2.ParamStruct()
                {
                    type = messageDic.GetType(),
                    value = messageDic
                }
            };

            await Protocol2.RequestAllMessages.SendCaller(Clients, temp);

            //await Clients.Caller.SendAsync(
            //    Protocol.Chat.ClientSide.RequestAllMessages.ProtoStr,
            //    JsonConvert.SerializeObject(temp));
        }
    }
}