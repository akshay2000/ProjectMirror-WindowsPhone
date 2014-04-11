using Microsoft.Live;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColorsWP8
{
    public class AuthenticationManager
    {
        public AuthenticationManager()
        {
            LogMeIn();
        }

        public event EventHandler AuthenticationCompleted;

        public async void LogMeIn()
        {
            await AuthenticateAsync();
            if(AuthenticationCompleted!=null)
            {
                AuthenticationCompleted(this, new EventArgs());
            }
        }

        public async Task AuthenticateAsync()
        {
            LiveAuthClient authClient = new LiveAuthClient("000000004C114D3D");
            LiveLoginResult result = await authClient.InitializeAsync(new[] { "wl.signin", "wl.offline_access" });
            LiveConnectSession session = result.Session;
            MobileServiceUser user = null;

            if (session == null)
            {
                result = await authClient.LoginAsync(new[] { "wl.signin", "wl.offline_access" });
                session = result.Session;
            }
            if (result.Status == LiveConnectSessionStatus.Connected)
            {
                string authToken = session.AuthenticationToken;
                user = await App.MirrorService.AuthenticateWithMicrosoftAsync(authToken);
            }
            else
            {
                Debug.WriteLine("Something went wrong with the authentication");
            }
        }
    }
}
