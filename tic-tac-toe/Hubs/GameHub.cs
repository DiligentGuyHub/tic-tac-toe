using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using tic_tac_toe.Models;

namespace tic_tac_toe.Hubs
{
    public class UserContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Connection> Connections { get; set; }
        public DbSet<ConversationRoom> Rooms { get; set; }
    }

    public class User
    {
        [Key]
        public string Username { get; set; }
        public ICollection<Connection> Connections { get; set; }
        public virtual ICollection<ConversationRoom> Rooms { get; set; }
    }

    public class Connection
    {
        public string ConnectionID { get; set; }
        public string UserAgent { get; set; }
        public bool Connected { get; set; }
    }

    public class ConversationRoom
    {
        [Key]
        public string RoomName { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }

    [Authorize]
    public class GameHub : Hub
    {
        List<User> Users = new List<User>();

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task CreateSession(string user)
        {
            await Clients.All.SendAsync("CreateSession", user);
        }
        public async Task SendTurn(string id, string turn)
        {
            //await Clients.Client(id).SendAsync("RecieveTurn", id, turn);
            await Clients.All.SendAsync("RecieveTurn", id, turn);
        }

        public override Task OnConnectedAsync()
        {
            using (var db = new UserContext())
            {
                // Retrieve user.
                var user = db.Users
                    .Include(u => u.Rooms)
                    .SingleOrDefault(u => u.Username == Context.User.Identity.Name);

                // If user does not exist in database, must add.
                if (user == null)
                {
                    user = new User()
                    {
                        Username = Context.User.Identity.Name
                    };
                    db.Users.Add(user);
                    db.SaveChanges();
                }
                else
                {
                    // Add to each assigned group.
                    foreach (var item in user.Rooms)
                    {
                        Groups.AddToGroupAsync(Context.ConnectionId, item.RoomName);
                    }
                }
            }
            return base.OnConnectedAsync();
        }

        public void AddToRoom(string roomName)
        {
            using (var db = new UserContext())
            {
                // Retrieve room.
                var room = db.Rooms.Find(roomName);

                if (room != null)
                {
                    var user = new User() { Username = Context.User.Identity.Name };
                    db.Users.Attach(user);

                    room.Users.Add(user);
                    db.SaveChanges();
                    Groups.AddToGroupAsync(Context.ConnectionId, roomName);
                }
            }
        }

        public void RemoveFromRoom(string roomName)
        {
            using (var db = new UserContext())
            {
                // Retrieve room.
                var room = db.Rooms.Find(roomName);
                if (room != null)
                {
                    var user = new User() { Username = Context.User.Identity.Name };
                    db.Users.Attach(user);

                    room.Users.Remove(user);
                    db.SaveChanges();

                    Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
                }
            }
        }
    }
}
