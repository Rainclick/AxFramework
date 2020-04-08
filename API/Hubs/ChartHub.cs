using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace API.Hubs
{
    public class ChartHub: Hub
    {
        public async Task SendMessage( string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }

        public async Task SendMessage(string clientId, string message)
        {
            await Clients.Clients(clientId).SendAsync("ReceiveMessage", message);
        }
    }
}
