using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WPFClient.View_Model;

namespace WPFClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public User User { get; set; }
        public MainWindow(User user)
        {
            DataContext = new MainViewModel() { UserNameMain = user.Name};
            User = user;
            InitializeComponent();
        }

        protected override void OnClosed(EventArgs e)
        {
            var mainVm = DataContext as MainViewModel;
            Task logout = Task.Run(async () => {
                await mainVm.LogoutUser(User).ConfigureAwait(false);
            });
            base.OnClosed(e);
        }
    }
}
