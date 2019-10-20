using BL.SignalRChat;
using Microsoft.AspNet.SignalR;
using SignalRChat.BL;
using System.Threading.Tasks;

namespace SignalRChat.Hubs
{
    public class ChatHub : Hub
    {
        private static UserBL _userBL = new UserBL(); 
        private static ChatBL _chatBL = new ChatBL();

        public async Task SignIn(string currentUser, string targetUser)
        {
            var userInDB = await _userBL.GetUser(currentUser);

            if (userInDB is null || userInDB.Online is false) return;


            var chat = _chatBL.FindChat(currentUser, targetUser);
            if(chat == null)
            {
                
                _chatBL.CreateChat(currentUser, targetUser, Context.ConnectionId);
            }
            else
            {
                chat.IsActive = true;
                chat.UserConnectionID = Context.ConnectionId;
                _chatBL.UpdateConnectionID(chat);
                _chatBL.UpDateChat(chat);
            }
     }

      

        public bool SendTo(string message, string currentUser, string targetUser)
        {
            var currentUserDBChat = _chatBL.FindChat(currentUser, targetUser);
            var targetUserDBChat = _chatBL.FindChat(targetUser, currentUser);

            

            if (targetUserDBChat.IsActive)
            {
                Clients.Client(targetUserDBChat.UserConnectionID).broadcastMessage(currentUserDBChat.User, message);
                return true;
            }

            else
                return false; // Will return false or not excpted
         
        }

        public void SignOut(string currentUser, string targetUser)
        {
            var currentUserDBChat = _chatBL.FindChat(currentUser, targetUser);

            if (currentUserDBChat == null) return;

            currentUserDBChat.IsActive = false;
            _chatBL.UpDateChat(currentUserDBChat);
        }

    }
}