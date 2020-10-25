using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace WebApi.Tools.SignalR
{
    /// <summary>
    /// 通过接口形式进行编译检查
    /// </summary>
    public class MessageHub : Hub<IChatClient>
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.ReceiveMessage(user, message);
        }

        public Task SendMessageToCaller(string message)
        {
            return Clients.Caller.ReceiveMessage(message);
        }
    }
}
