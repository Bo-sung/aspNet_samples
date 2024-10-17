using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace aspnetChat_server.Protocols
{
    // Server Only
    public partial class Protocol
    {
        /// <summary>
        /// 접속중인 전체 클라에게 전송
        /// </summary>
        /// <param name="clients"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task SendAll(IHubCallerClients clients, object obj)
        {
            if (clients == null)
                return;
            if (obj == null)
                return;
            string paramStr = JsonConvert.SerializeObject(obj);
            await clients.All.SendAsync(m_protocolStr, paramStr);
        }

        /// <summary>
        /// 해당 프로토콜 호출한 클라에게 전송
        /// </summary>
        /// <param name="clients"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task SendCaller(IHubCallerClients clients, object obj)
        {
            if (clients == null)
                return;
            if (obj == null)
                return;
            string paramStr = JsonConvert.SerializeObject(obj);
            await clients.Caller.SendAsync(m_protocolStr, paramStr);
        }

        /// <summary>
        /// 프로토콜 파라미터 JSON 문자열로 변환
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string SerializeParam(Param param)
        {
            return JsonConvert.SerializeObject(param);
        }

        /// <summary>
        /// 프로토콜 파라미터 JSON 문자열을 파라미터 오브젝트로 변환
        /// </summary>
        /// <param name="paramStr"></param>
        /// <returns></returns>
        public static Param? DeserializeParam(string paramStr)
        {
            return JsonConvert.DeserializeObject<Param>(paramStr);
        }
    }
}
