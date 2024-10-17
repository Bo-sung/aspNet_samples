using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace aspnetChat_server.Protocols
{
    // Server Only
    public partial class Protocol
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

        public static string SerializeParam(Param param)
        {
            return JsonConvert.SerializeObject(param);
        }

        public static Param? DeserializeParam(string paramStr)
        {
            return JsonConvert.DeserializeObject<Param>(paramStr);
        }
    }
}
