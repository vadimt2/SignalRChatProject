using Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WPFClient.BL;
using WPFClient.Commands;

namespace WPFClient.View_Model
{
    public class ChatViewModel : BaseViewModel, IDisposable
    {
        private readonly ChatBL _chatBL;
        private Chat chat;
        public CommandExecuter SendMessageCommand { get; private set; }

        public ObservableCollection<string> Messages { get; private set; } = new ObservableCollection<string>();

        private string _message;
        public string Message
        {
            get => _message;
            private set
            {
                _message = value;
                OnPropertyChanged();
            }
        }
        private string _userName;
        public string UserName
        {
            get => _userName;
            private set
            {
                if (_userName == value) return;

                _userName = value;
                OnPropertyChanged();
            }
        }
        public string TargetUser { get; private set; }


        public ChatViewModel(string userName, string targetUser)
        {
            _chatBL = new ChatBL();
            _userName = userName;
            TargetUser = targetUser;
            _chatBL.ConnectToServer(AddMessage);
            SignIn(_userName, targetUser);
             chat = new Chat(userName, targetUser);
            SendMessageCommand = new CommandExecuter(SendMessage);
        }

        // At the ctor will get the user right away
        private void SignIn(string userName, string targetUser)
        {
            _chatBL.SignIn(userName, targetUser);
        }

        // Rasive message from the button command
        private void SendMessage()
        {
            if (string.IsNullOrWhiteSpace(Message)) return;

            var currentUserMessage = $"Me: {Message}";
            Messages.Add(currentUserMessage);
            var accepted = _chatBL.SendMessage(Message, UserName, TargetUser);
            
            accepted.ConfigureAwait(true);
            if (!accepted.Result)
                Messages.Add($"User: {TargetUser} is not connected to chat, Message: {Message}, not Recived!");
            Message = String.Empty;
        }

        // My method that coming from the ChatBL BY Delagate !
        public void AddMessage(string userName, string message)
        {
            var stringMessage = $"{userName}: {message}";
            Messages.Add(stringMessage);
            OnPropertyChanged(nameof(Messages));
        }

        ~ChatViewModel()
        {
            Dispose();
        }

        public void Dispose()
        {
            _chatBL.SignOut(chat.User, chat.Target);
        }

      
    }
}
