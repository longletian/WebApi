using System.Threading.Tasks;

namespace WebApi.Tools.SignalR
{
   public interface IChatClient
    {
        Task ReceiveMessage(string user, string message);
        Task ReceiveMessage(string message);
    }
}
