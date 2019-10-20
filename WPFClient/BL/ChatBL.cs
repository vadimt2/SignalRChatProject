using Common;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WPFClient.BL
{
    public class ChatBL
    {
        private HubConnection hubConnection;
        private IHubProxy chatHubProxy;

        public void ConnectToServer(Action<string,string> AddMessage)
        {
            //Server methods should be called on non UI thread
            Task connectTask = Task.Run(async () =>
            {
                hubConnection = new HubConnection("http://localhost:52527/");
                chatHubProxy = hubConnection.CreateHubProxy("ChatHub");
                chatHubProxy.On("broadcastMessage", (string userName, string message) =>
                {
                    //View Model methods should run on the main thread
                    Application.Current.Dispatcher.InvokeAsync(
                               () =>
                               {
                                   AddMessage(userName, message);
                               });
                });
                await hubConnection.Start();
            });
            connectTask.ConfigureAwait(false);//Does not return to and deadlocks the UI thread after execution
            connectTask.Wait();
        }

        public void SignIn(string currentUser, string targetUser)
        {
            chatHubProxy.Invoke("SignIn", currentUser, targetUser);
        }

        public async Task<bool> SendMessage(string message, string currentUser, string targetUser)
        {
            Task<bool> result = chatHubProxy.Invoke<bool>("SendTo", message, currentUser, targetUser);

           var resAwait = await result.ConfigureAwait(false);

                               return resAwait is true ? true : false;             
        }

        public void SignOut(string currentUser, string targetUser)
        {
            chatHubProxy.Invoke("SignOut", currentUser, targetUser);
        }

    }
}
