using BL.SignalRChat;
using Common;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Hubs.SignalRChat
{
    public class UserHub : Hub
    {
        static UserBL _userBL;

        public UserHub()
        {
            _userBL = new UserBL();
        }
       
        public async Task<string> Register(User name)
        {
            return await _userBL.Register(name, UserConnected);
        }

        public async Task<string> Login(User name)
        {
            return await _userBL.Login(name, UserConnected).ConfigureAwait(false);
        }

        public async Task<string> Logout(User user)
        {
            return await _userBL.Logout(user, UserDisccouncted);
        }

      
        public void UserConnected(string userName)
        {
            this.Clients.All.LogInNotificated(userName);
        }

        public void UserDisccouncted(string userName)
        {
            this.Clients.All.LogOutNotificated(userName);
        }

        public async Task<IEnumerable<string>> GetAllUsers()
        {
            return await _userBL.GetAllUsers();
        }

        public async Task<IEnumerable<string>> GetOnlineUsers()
        {
            return await _userBL.GetAllOnllineUsers();
        }

        public async Task<IEnumerable<string>> GetOfflineUsers()
        {
            return await _userBL.GetAllOfflineUsers();
        }

        public async Task<User> GetUser(string userName)
        {
            return await _userBL.GetUser(userName);
        }
    }
}