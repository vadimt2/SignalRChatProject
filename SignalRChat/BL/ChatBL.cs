using Common;
using DAL.SignalRChat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRChat.BL
{
    public class ChatBL
    {
        private readonly DB _db;
        public ChatBL()
        {
            _db = new DB();
        }

        public Chat ChetExist(Chat chat)
        {
            var chatResult = _db.Chats.SingleOrDefault(chatDB => chatDB.User == chat.User && chatDB.Target == chat.Target);
            if (chatResult != null) return chatResult;

            var chatRes = _db.Chats.SingleOrDefault(chatDB => chatDB.User == chat.Target && chatDB.Target == chat.User);
            if (chatRes != null) return chatRes;

            return null;

        }

        public Chat FindChat(string user, string targetUser)
        {
            var chatResult = _db.Chats.SingleOrDefault(chatDB => chatDB.User == user && chatDB.Target == targetUser);
            if (chatResult != null) return chatResult;

            return null;

        }

        public Chat CreateChat(string user, string targetUser, string connectionId)
        {
            Chat chat = new Chat(user, targetUser) { Guid = Guid.NewGuid().ToString(), UserConnectionID = connectionId };
            chat.IsActive = true;
            _db.Chats.Add(chat);
            _db.SaveChanges();
            return chat;
        }

        public Chat UpdateConnectionID(Chat chat)
        {
            var chatResult = _db.Chats.SingleOrDefault(chatDB => chatDB.User == chat.User && chatDB.Target == chat.Target);

            if (chat == null) return null;

            chatResult.UserConnectionID = chat.UserConnectionID;
            _db.SaveChanges();
            return chatResult;
        }

        public void UpDateChat(Chat chat)
        {
            var chatResult = _db.Chats.SingleOrDefault(chatDB => chatDB.User == chat.User && chatDB.Target == chat.Target);
            if (chatResult == null) return;
            chatResult.IsActive = chat.IsActive;
            _db.SaveChanges();
        }



    }
}