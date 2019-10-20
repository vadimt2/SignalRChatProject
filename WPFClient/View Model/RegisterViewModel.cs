using BL.WPFClient;
using Common;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using WPFClient.Commands;

namespace WPFClient.View_Model
{
    public class RegisterViewModel : BaseViewModel
    {
        private readonly UserBL _bl;

        public CommandExecuter CreateUserCommand { get; private set; }

        public ObservableCollection<string> Users { get; protected set; }

        private string userName;
        private string password;

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
                CreateUserCommand.NotifyCanExecuteChanged();
            }
        }
        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                password = value;
                OnPropertyChanged();
                CreateUserCommand.NotifyCanExecuteChanged();
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


        public RegisterViewModel()
        {
            _bl = new UserBL();
            Users = new ObservableCollection<string>();
            _bl.ConnectToServer(OnCreatedUser);
            Task loadUsers = LoadUsers();
            CreateUserCommand = new CommandExecuter(RegisterUser, () => { return !string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(Password); });
        }

        private async Task LoadUsers()
        {
            IEnumerable<string> userList = await _bl.GetAllUsers();
            Users = new ObservableCollection<string>(userList);
            OnPropertyChanged(nameof(Users));
        }

        public void OnCreatedUser(string userName)
        {
            Users.Add(userName);
            OnPropertyChanged(nameof(Users));
        }

        private async void RegisterUser()
        {
            if (!string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(Password))
            {
                User user = new User(UserName, Password);
                Error = await _bl.Register(user);
            }
        }


    }
}
