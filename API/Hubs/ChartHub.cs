using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace API.Hubs
{
    public class AxHub : Hub
    {
        public async Task SendMessageToAll(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }

        public async Task SendMessage(string clientId, string message)
        {
            await Clients.Clients(clientId).SendAsync("ReceiveMessage", message);
        }

        public async Task UpdateChart(string clientId, object data, int chartId)
        {
            await Clients.Clients(clientId).SendAsync("UpdateChart", data, chartId);
        }

        public string GetConnectionId()
        {
            return Context.ConnectionId;
        }



    }
}
