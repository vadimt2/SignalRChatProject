using BL.WPFClient;
using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WPFClient.Commands;

namespace WPFClient.View_Model
{
    public class LoginViewModel : BaseViewModel
    {
        internal readonly UserBL _bl;

        public CommandExecuter LoginUserCommand { get; private set; }
        public CommandExecuter RegisterUserCommand { get; private set; }

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
                LoginUserCommand.NotifyCanExecuteChanged();
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
                LoginUserCommand.NotifyCanExecuteChanged();
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

        public LoginViewModel()
        {
            _bl = new UserBL();

            LoginUserCommand = new CommandExecuter(LoginUser, () => { return !string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(Password); });

            RegisterUserCommand = new CommandExecuter(NavigateToRegisterUser);
        }

        public void LoginUser()
        {
            if (!string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(Password))
            {
                User user = new User(UserName, Password);

                Error = _bl.Login(user).Result;
                
                if (Error == "")
                {
                    Application.Current.Properties.Add(nameof(UserName), user);
                    MainWindow mainWindow = new MainWindow(user);
                    mainWindow.ShowDialog();
                }
            }
        }

        private void NavigateToRegisterUser()
        {
            RegisterWindow registerWindow = new RegisterWindow();

            registerWindow.ShowDialog();
        }

    }
}
