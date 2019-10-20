using BL.WPFClient;
using Common;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;

using WPFClient.Commands;
using WPFClient.Views;

namespace WPFClient.View_Model
{
    public class MainViewModel : BaseViewModel
    {
        private readonly UserBL _userBl;

        public CommandExecuter ChatCommand { get; private set; }

        private readonly List<string> chatList;

        public string UserNameMain { get; set; }

        private string _targetUser;

        public string TargetUser
        {
            get
            {
                return _targetUser;
            }
            set
            {
                _targetUser = value;
                OnPropertyChanged("ListBoxUser");
            }
        }

        public ObservableCollection<string> OnlineUsers { get; private set; }

        public ObservableCollection<string> OfflineUsers { get; private set; }


        public MainViewModel()
        {
            _userBl = new UserBL();

            _userBl.ConnectToServer(OnUserLoggedIn);
            Task disconnected = _userBl.DisConnectToServer(UserLoggedOut);

            Task TaskLoadOnlineUsers = Task.Run(async () => {
                await LoadOnlineUsers().ConfigureAwait(false);
            });

            Task TaskLoadOfflineUsers = Task.Run(async () => {
                await LoadOfflineUsers().ConfigureAwait(false);
            });
            
            chatList = new List<string>();
            ChatCommand = new CommandExecuter(StartChat, () => { return true; });
        }

        public void StartChat()
        {
            var userInChatList = chatList.SingleOrDefault(x => x == TargetUser);

            if (userInChatList != null) return;

            chatList.Add(TargetUser);
            ChatWindow chatWindow = new ChatWindow(UserNameMain, TargetUser, RemoveFromChatList);
            chatWindow.Show();
        }

        public void RemoveFromChatList(string targetUser)
        {
            var userInChatList = chatList.SingleOrDefault(x => x == targetUser);
            if (userInChatList == null) return;
            chatList.Remove(userInChatList);
        }

        private async Task LoadOnlineUsers()
        {
            IEnumerable<string> userList = await _userBl.GetOnlineUsers().ConfigureAwait(false);
            OnlineUsers = new ObservableCollection<string>(userList);
            OnlineUsers.Remove(UserNameMain);
            OnPropertyChanged(nameof(OnlineUsers));
        }

        private async Task LoadOfflineUsers()
        {
            IEnumerable<string> userList = await _userBl.GetOfflineUsers().ConfigureAwait(false);
            OfflineUsers = new ObservableCollection<string>(userList);
            OnPropertyChanged(nameof(OfflineUsers));
        }


        public void OnUserLoggedIn(string userName)
        {
            if (userName != null)
                OfflineUsers.Remove(userName);

            var userOnline = OnlineUsers.SingleOrDefault(x => x == userName);


            if (userOnline != null) return;

            OnlineUsers.Add(userName);
            OnPropertyChanged(nameof(OnlineUsers));
        }

        public void UserLoggedOut(string userName)
        {
            OnlineUsers.Remove(userName);
            OfflineUsers.Add(userName);
            OnPropertyChanged(nameof(OfflineUsers));

        }

        public async Task LogoutUser(User user)
        {
            Application.Current.Properties.Remove("UserName");
            await _userBl.Logout(user);
        }

    }
}
