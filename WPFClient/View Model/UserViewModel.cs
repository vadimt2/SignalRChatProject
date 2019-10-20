using BL.WPFClient;
using Common;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using WPFClient;
using WPFClient.Commands;

namespace View_Model.WPFClient
{
    class UserViewModel : INotifyPropertyChanged
    {
        //List of Existing User
        public ObservableCollection<string> Users { get; private set; }

        internal readonly UserBL _bl;

        public event PropertyChangedEventHandler PropertyChanged;

        public UserViewModel()
        {
            _bl = new UserBL();
        }

        private string userName;
        public string UserName
        {
            get
            {
                return userName;
            }
            set
            {
                userName = value;
                OnPropertyChanged();
            }
        }

        private string error;
        public string Error
        {
            get
            {
                return error;
            }
            set
            {
                error = value;
                OnPropertyChanged();
            }
        }

        public async Task LogoutUser(User user)
        {
            Application.Current.Properties.Remove(nameof(UserName));
            Error = await _bl.Logout(user);
        }

      

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
