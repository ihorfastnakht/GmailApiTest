using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace testApp
{
    public sealed partial class MainPage : Page
    {
        int index = 1;
        public List<UserCredential> Credentials { get; } = new List<UserCredential>();

        public MainPage()
        {
            this.InitializeComponent();
            this.Loaded += async (s, e) =>
            {
            };
        }

        public async Task<UserCredential> GetCredential()
        {
            var scopes = new[] { GmailService.Scope.GmailModify };
            var clientSecrets = new ClientSecrets()
            {
                ClientId = "733025278625-9nj4pg86tnu95kthh1k78c9c3caj6cov.apps.googleusercontent.com",
                ClientSecret = "JBhxhgvJCIqW6gPEYRY3C-Xp"
            };
            var credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(clientSecrets, scopes, $"user{index}", CancellationToken.None);
            return credential;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var cred = await GetCredential();
                Credentials.Add(cred);
                index++;
                list.ItemsSource = Credentials;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: " + ex.Message);
            }
        }
    }
}
