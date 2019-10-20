using System;
using System.Windows;
using WPFClient.View_Model;

namespace WPFClient.Views
{
    /// <summary>
    /// Interaction logic for ChatWindow.xaml
    /// </summary>
    public partial class ChatWindow : Window
    {
        private ChatViewModel _chat;
        public bool IsOn { get; set; }

        private readonly Action<string> _removeFromChatList;
        public ChatWindow(string user, string targetUser, Action<string> removeFromChatList)
        {
             IsOn = true;
            _chat = new ChatViewModel(user, targetUser);
            DataContext = _chat;
            _removeFromChatList = removeFromChatList;
            InitializeComponent();
        }

        protected override void OnClosed(EventArgs e)
        {
            IsOn = false;
            _chat.Dispose();
            _removeFromChatList.Invoke(_chat.TargetUser);
            base.OnClosed(e);
        }

    }
}
