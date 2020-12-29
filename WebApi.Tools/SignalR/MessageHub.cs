using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace WebApi.Tools.SignalR
{

    [Authorize("SignalRAuthorizationPolicy")]
    /// <summary>
    /// 通过接口形式进行编译检查(创建和使用集线器)
    /// </summary>
    public class MessageHub : Hub<IChatClient>
    {
        //await调用依赖于中心保持活动状态的异步方法时使用

        #region 三种集线器方法
        //SendMessage 使用将消息发送到所有连接的客户端 Clients.All 。
        //SendMessageToCaller 使用将消息发回给调用方 Clients.Caller 。
        //SendMessageToGroups 向组中的所有客户端发送一条消息 SignalR Users
        #endregion

        //更改集线器方法的名称
        [HubMethodName("SendMessageToUser")]
        public async Task SendMessage(string user, string message)
        {
       
            await Clients.All.ReceiveMessage(user, message);
        }

        public Task SendMessageToCaller(string message)
        {
            return Clients.Caller.ReceiveMessage(message);
        }

        /// <summary>
        /// 处理连接事件( 重写 OnConnectedAsync 虚拟方法，
        /// 以便在客户端连接到集线器时执行操作，如将其添加到组。)
        /// </summary>
        /// <returns></returns>
        public async override Task OnConnectedAsync()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "SignalR Users");

            await base.OnConnectedAsync();
        }
        /// <summary>
        /// 以便在客户端断开连接时执行操作(如果客户端通过调用 (特意断开连接 connection.stop() ，
        /// 例如) ，则 exception 参数将为 null 。 
        /// 但是，如果客户端由于错误而断开连接 (例如，) 网络故障，则 exception 参数将包含描述失败的异常。)
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }

        /// <summary>
        ///  要将 异常情况传播到客户端，则可以使用 HubException 类
        /// </summary>
        /// <returns></returns>
        public Task ThrowException()
        {
            throw new HubException("This error will be sent to the client!");
        }
    }
}
