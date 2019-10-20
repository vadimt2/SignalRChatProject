using Common;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BL.WPFClient
{
    public class UserBL
    {
        private HubConnection hubConnection;
        private IHubProxy userHubProxy;

        public UserBL()
        {
            Task connectTask = Task.Run(async () =>
            {
                hubConnection = new HubConnection("http://localhost:52527/");
                userHubProxy = hubConnection.CreateHubProxy("UserHub");
                await hubConnection.Start();
            });

            connectTask.Wait();
        }

        public void ConnectToServer(Action<string> OnUserLoggedIn)
        {
            //Server methods should be called on non UI thread
            Task connectTask = Task.Run(async () =>
           {
               hubConnection = new HubConnection("http://localhost:52527/");
               userHubProxy = hubConnection.CreateHubProxy("UserHub");
               userHubProxy.On<string>("LogInNotificated", (string userName) =>
               {
                   //View Model methods should run on the main thread
                   Application.Current.Dispatcher.InvokeAsync(
                             () =>
                             {
                                  // Code to run on the GUI thread.
                                  OnUserLoggedIn(userName);
                             });
               });
               await hubConnection.Start();
           });
            connectTask.Wait();
        }

        public async Task DisConnectToServer(Action<string> OnUserLoggedOut)
        {
            hubConnection = new HubConnection("http://localhost:52527/");
            userHubProxy.On<string>("LogOutNotificated", (string userName) =>
            {
                if (Application.Current != null)
                    Application.Current.Dispatcher.InvokeAsync(
                                  () =>
                                  {
                                      // Code to run on the GUI thread.
                                      OnUserLoggedOut(userName);
                                  });
            });
            await hubConnection.Start();
        }

        public async Task<string> Register(User user)
        {

            string error = await userHubProxy.Invoke<string>("Register", user).ConfigureAwait(false); ;
            return error;
        }

        public async Task<string> Login(User user)
        {
            string error = await userHubProxy.Invoke<string>("Login", user).ConfigureAwait(false); ;
            return error;
        }

        public async Task<string> Logout(User user)
        {
            string error = await userHubProxy.Invoke<string>("Logout", user).ConfigureAwait(false); ;
            return error;
        }

        public async Task<IEnumerable<string>> GetAllUsers()
        {
            //Server methods should be called on non UI thread
            Task<IEnumerable<string>> getUsersTask = Task.Run(async () =>
            {
                IEnumerable<string> error = await userHubProxy.Invoke<IEnumerable<string>>("GetAllUsers");
                return error;
            });
            await getUsersTask.ConfigureAwait(false);//Does not return to and deadlocks the UI thread after execution
            return await getUsersTask;

        }

        public async Task<IEnumerable<string>> GetOnlineUsers()
        {
            IEnumerable<string> error = await userHubProxy.Invoke<IEnumerable<string>>("GetOnlineUsers").ConfigureAwait(false); ;
            return error;

        }
        public async Task<IEnumerable<string>> GetOfflineUsers()
        {

            IEnumerable<string> error = await userHubProxy.Invoke<IEnumerable<string>>("GetOfflineUsers").ConfigureAwait(false); ;
            return error;

        }


    }
}
