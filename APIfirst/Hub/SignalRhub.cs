using Microsoft.AspNetCore.SignalR;
namespace APIfirst
{
    public class SignalRhub : Hub
    {

        /// <summary>
        ///  实现消息的发送，接受消息后发送消息给其他客户端
        /// </summary>
        /// <param name="user"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
        public override Task OnConnectedAsync() {
           return base.OnConnectedAsync();
        }
    }
}


