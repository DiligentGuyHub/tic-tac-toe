using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace tic_tac_toe.Hubs
{
    public class GameHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task CreateSession(string user)
        {
            //byte[] bytes = new byte[16];
            //using (var rnd = new RNGCryptoServiceProvider())
            //{
            //    rnd.GetBytes(bytes);
            //}
            //string connectionID = BitConverter.ToString(bytes);
            //string connection = user1.GetHashCode() > user2.GetHashCode() ?
            //    $"{user1}_{user2}":
            //    $"{user2}_{user1}";
            await Clients.All.SendAsync("CreateSession", user);
        }
        public async Task SendTurn(string id, string turn)
        {
            await Clients.Client(id).SendAsync("RecieveTurn", turn);
            //await Clients.All.SendAsync("RecieveTurn", id, turn);
        }
    }
}
